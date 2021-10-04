using Microsoft.Reporting.WebForms;
using System;

namespace DITS.HILI.WMS.Web.apps.Report
{
    public class ReportViewerDisposer : IDisposable
    {

        // Fields  
        private bool _CollectGarbageOnDispose = true;
        private readonly ReportViewer _ReportViewer;
        private bool disposedValue = false;
        private const string EVENTHANDLER_ON_USER_PREFERENCE_CHANGED = "OnUserPreferenceChanged";
        private const string LIST_HANDLERS = "_handlers";
        private const string ON_USER_PREFERENCE_CHANGED_EVENT = "OnUserPreferenceChangedEvent";
        private const string SYSTEM_EVENT_INVOKE_INFO = "SystemEventInvokeInfo";
        private const string TARGET_DELEGATE = "_delegate";
        private const string TOOLSTRIP_CONTROL_NAME = "reportToolBar";
        private const string TOOLSTRIP_TEXTBOX_CONTROL_NAME_CURRENT_PAGE = "currentPage";
        private const string TOOLSTRIP_TEXTBOX_CONTROL_NAME_TEXT_TO_FIND = "textToFind";

        // Methods  
        public ReportViewerDisposer(ReportViewer rptv)
        {
            if (rptv == null)
            {
                throw new ArgumentNullException("ReportViewer cannot be null.");
            }
            _ReportViewer = rptv;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue && disposing)
            {
                _ReportViewer.Dispose();
                if (_CollectGarbageOnDispose)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
            }
            disposedValue = true;
        }
        // Properties  
        public bool CollectGarbageOnDispose
        {
            get => _CollectGarbageOnDispose;
            set => _CollectGarbageOnDispose = value;
        }
    }
}
