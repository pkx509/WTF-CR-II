using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;

namespace DITS.HILI.WMS.Web.apps.Report
{
    public partial class frmReportViewer : BaseUIPage
    {
        #region Global Variables

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string m_printerName;
        private readonly string m_server;
        private readonly string m_path;
        private readonly string m_name;
        private readonly Dictionary<string, string> m_parameters;
        private SizeF m_pageSize;
        private float m_marginLeft;
        private float m_marginTop;
        private readonly float m_marginRight;
        private readonly float m_marginBottom;
        private readonly short m_copies;

        private int m_currentPageIndex;
        private List<Stream> m_reportStreams;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            bool silent = false;
            log4net.Config.XmlConfigurator.Configure();

            if (!Page.IsPostBack)
            {
                string reportName = Request.QueryString["reportName"].ToString();
                string tmpSilent = Request.QueryString["Silent"] == null ? "" : Request.QueryString["Silent"].ToString();
                m_printerName = Request.QueryString["PrinterName"] == null ? "" : Request.QueryString["PrinterName"].ToString();

                string tmpWidth = Request.QueryString["width"] == null ? "" : Request.QueryString["width"].ToString();
                string tmpHeight = Request.QueryString["height"] == null ? "" : Request.QueryString["height"].ToString();

                float tw, th;
                tw = th = 0F;

                float.TryParse(tmpWidth, out tw);
                float.TryParse(tmpHeight, out th);

                if (tw == 0 && th == 0)
                {
                    m_pageSize.Width = 4.5F;
                    m_pageSize.Height = 6.8F;
                }
                else
                {
                    m_pageSize.Width = tw;
                    m_pageSize.Height = th;
                }

                m_marginLeft = 0.5F;
                m_marginTop = 0.5F;

                bool.TryParse(tmpSilent, out silent);

                StringBuilder sb = new StringBuilder();
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                foreach (object s in Request.QueryString)
                {
                    if (s.ToString() != "reportName" && s.ToString() != "_dc" && s.ToString() != "Silent" && s.ToString() != "PrinterName" && s.ToString() != "width" && s.ToString() != "height")
                    {
                        list.Add(new KeyValuePair<string, string>(s.ToString(), Request.QueryString[s.ToString()].ToString()));
                        //sb.AppendFormat("&{0}={1}", s.ToString(), Request.QueryString[s.ToString()]);
                    }
                }

                string rptPath = System.Configuration.ConfigurationManager.AppSettings["ReportingService"];
                string rptFolder = System.Configuration.ConfigurationManager.AppSettings["ReportingServiceFolder"];
                string rptUser = System.Configuration.ConfigurationManager.AppSettings["ReportingUser"];
                string rptPassword = System.Configuration.ConfigurationManager.AppSettings["ReportingPassword"];
                string rptDomain = System.Configuration.ConfigurationManager.AppSettings["ReportingDomain"];

                if (silent)
                {

                    PrintSilent(reportName, sb, list, rptPath, rptFolder, rptUser, rptPassword, rptDomain);
                    Page.ClientScript.RegisterStartupScript(GetType(), "myCloseScript", "var win=window.open('','_self');win.close();", true);
                }
                else
                {
                    IReportServerCredentials irsc = new CustomReportCredentials(rptUser, rptPassword, rptDomain);
                    ReportViewer1.ProcessingMode = ProcessingMode.Remote;
                    ReportViewer1.ServerReport.ReportServerCredentials = irsc;
                    ReportViewer1.ServerReport.ReportServerUrl = new Uri(rptPath);
                    ReportViewer1.ServerReport.ReportPath = rptFolder + reportName + sb.ToString();

                    if (list.Count > 0)
                    {
                        int i = 0;
                        ReportParameter[] reportParameterCollection = new ReportParameter[list.Count];
                        foreach (KeyValuePair<string, string> k in list)
                        {
                            reportParameterCollection[i] = new ReportParameter(k.Key, k.Value);
                            i++;
                        }
                        try
                        {
                            ReportViewer1.ServerReport.SetParameters(reportParameterCollection);
                            ReportViewer1.ServerReport.Refresh();
                        }
                        catch { }
                    }
                }
            }
        }

        private void PrintSilent(string reportName, StringBuilder sb, List<KeyValuePair<string, string>> list, string rptPath, string rptFolder, string rptUser, string rptPassword, string rptDomain)
        {

            ReportViewer viewer = new ReportViewer();
            ReportViewerDisposer disposer = new ReportViewerDisposer(viewer);

            IReportServerCredentials irsc = new CustomReportCredentials(rptUser, rptPassword, rptDomain);
            viewer.ServerReport.ReportServerCredentials = irsc;
            viewer.ServerReport.ReportServerUrl = new Uri(rptPath);
            viewer.ServerReport.ReportPath = rptFolder + reportName + sb.ToString();

            if (list.Count > 0)
            {
                int i = 0;
                ReportParameter[] reportParameterCollection = new ReportParameter[list.Count];
                foreach (KeyValuePair<string, string> k in list)
                {
                    reportParameterCollection[i] = new ReportParameter(k.Key, k.Value);
                    i++;
                }
                viewer.ServerReport.SetParameters(reportParameterCollection);
                viewer.ServerReport.Refresh();

            }
            try
            {
                CultureInfo us = new CultureInfo("en-US");
                string deviceInfo = string.Format(
                  "<DeviceInfo>" +
                  "  <OutputFormat>EMF</OutputFormat>" +
                  "  <PageWidth>{0}in</PageWidth>" +
                  "  <PageHeight>{1}in</PageHeight>" +
                  "  <MarginTop>{2}in</MarginTop>" +
                  "  <MarginLeft>{3}in</MarginLeft>" +
                  "  <MarginRight>{4}in</MarginRight>" +
                  "  <MarginBottom>{5}in</MarginBottom>" +
                  "</DeviceInfo>",
                  Math.Round(m_pageSize.Width, 2).ToString(us),
                  Math.Round(m_pageSize.Height, 2).ToString(us),
                  Math.Round(m_marginTop, 2).ToString(us),
                  Math.Round(m_marginLeft, 2).ToString(us),
                  Math.Round(m_marginRight, 2).ToString(us),
                  Math.Round(m_marginBottom, 2).ToString(us));

                //logger.Info($"Initial width = {m_pageSize.Width} , height = {m_pageSize.Height} , top = {m_marginTop} , left = {m_marginLeft} , right = {m_marginRight} , bottom = {m_marginBottom}");

                m_reportStreams = new List<Stream>();
                try
                {
                    NameValueCollection urlAccessParameters = new NameValueCollection
                    {
                        { "rs:PersistStreams", "True" }
                    };

                    Stream s = viewer.ServerReport.Render("IMAGE", deviceInfo, urlAccessParameters, out string mime, out string extension);
                    m_reportStreams.Add(s);

                    urlAccessParameters.Remove("rs:PersistStreams");
                    urlAccessParameters.Add("rs:GetNextStream", "True");
                    do
                    {
                        s = viewer.ServerReport.Render("IMAGE", deviceInfo, urlAccessParameters, out mime, out extension);
                        if (s.Length != 0)
                        {
                            m_reportStreams.Add(s);
                        }
                    }
                    while (s.Length > 0);

                    DoPrint();

                }
                finally
                {
                    foreach (Stream s in m_reportStreams)
                    {
                        s.Close();
                        s.Dispose();
                    }
                    m_reportStreams = null;

                }
            }
            finally
            {
                disposer.CollectGarbageOnDispose = true;
                disposer.Dispose();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.EnablePartialRendering = false;
            }
        }

        private void DoPrint()
        {
            m_currentPageIndex = 0;

            PrintDocument printDoc = new PrintDocument();
            try
            {
                printDoc.PrintController = new StandardPrintController();
                printDoc.PrinterSettings.PrinterName = m_printerName;
                printDoc.PrinterSettings.Copies = m_copies;

                if (!printDoc.PrinterSettings.IsValid)
                {
                    throw new ArgumentException(string.Format("Printer Name '{0}' invalid!", m_printerName));
                }


                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                printDoc.QueryPageSettings += new QueryPageSettingsEventHandler(QueryPageSettings);
                printDoc.Print();
            }
            finally
            {
                printDoc.PrintPage -= new PrintPageEventHandler(PrintPage);
                printDoc.QueryPageSettings -= new QueryPageSettingsEventHandler(QueryPageSettings);
                printDoc.Dispose();
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            if (m_currentPageIndex < m_reportStreams.Count)
            {
                Metafile mf = new Metafile(m_reportStreams[m_currentPageIndex++]);
                try
                {
                    ev.Graphics.DrawImage(mf, ev.PageBounds);
                }
                finally
                {
                    mf.Dispose();
                }
            }
            //ev.HasMorePages = m_currentPageIndex < m_reportStreams.Count;
            ev.HasMorePages = false;
        }

        private void QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {
            //logger.Info($"width : {m_pageSize.Width} and height : {m_pageSize.Height}");
            //logger.Info($"IsLandscape = {e.PageSettings.Landscape}");
            e.PageSettings.Landscape = m_pageSize.Width > m_pageSize.Height;
            //e.PageSettings.Landscape = false;
        }
    }

    public class CustomReportCredentials : IReportServerCredentials
    {
        private readonly string _UserName;
        private readonly string _PassWord;
        private readonly string _DomainName;

        public CustomReportCredentials(string UserName, string PassWord, string DomainName)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            _DomainName = DomainName;
        }

        public System.Security.Principal.WindowsIdentity ImpersonationUser => null;

        public ICredentials NetworkCredentials => new NetworkCredential(_UserName, _PassWord, _DomainName);

        public bool GetFormsCredentials(out Cookie authCookie, out string user,
         out string password, out string authority)
        {
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }


}

