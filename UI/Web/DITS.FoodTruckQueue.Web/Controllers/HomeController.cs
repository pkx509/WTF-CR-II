using DITS.FoodTruckQueue.Web.Hubs;
using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.MasterModel.Secure;
using DITS.HILI.WMS.TruckQueueModel;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

public class HomeController : Controller
{
    private readonly string _soundPath = ConfigurationManager.AppSettings["SoundPath"].ToString();
    private readonly string _tmpSoundPath = "../Sound/Temp";
    public HomeController()
    {
        if (DITS.HILI.WMS.ClientService.Common.User == null)
        {
            AutoLogon();
        }
    }
    [Route("Home")]
    [Route("")]
    [Route("Index")]
    public ActionResult Index()
    {

        return View();
    }
    [HttpGet]
    [Route("GetQueueConfiguration")]
    public ActionResult GetQueueConfiguration()
    {
        try
        {
            if (DITS.HILI.WMS.ClientService.Common.User == null)
            {
                var json = Json(new QueueConfiguration()
                {
                    EnableMessage = false,
                    Message = ""
                }, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            else
            {
                DITS.HILI.WMS.Core.Domain.ApiResponseMessage apiResp = DITS.HILI.WMS.ClientService.Queue.QueueClient.GetConfigurationActive().Result;
                if (apiResp.ResponseCode == "0")
                {
                    var data = apiResp.Get<QueueConfiguration>();
                    var json = Json(data, JsonRequestBehavior.AllowGet);
                    json.MaxJsonLength = int.MaxValue;
                    return json;
                }
                else
                {
                    var json = Json(new QueueConfiguration()
                    {
                        EnableMessage = false,
                        Message = ""
                    }, JsonRequestBehavior.AllowGet);
                    json.MaxJsonLength = int.MaxValue;
                    return json;
                }
            }
        }
        catch (Exception ex)
        {
            var json = Json(new QueueConfiguration()
            {
                EnableMessage = false,
                Message = ex.Message
            }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
    }
    [HttpGet]
    [Route("GetInQueue")]
    public ActionResult GetInQueue()
    {
        var docks = new List<QueueDock>()
                {
                     new QueueDock(){ QueueDockName="ช่องที่ 0" },
                     new QueueDock(){ QueueDockName="ช่องที่ 1" },
                     new QueueDock(){ QueueDockName="ช่องที่ 2" },
                     new QueueDock(){ QueueDockName="ช่องที่ 3" },
                     new QueueDock(){ QueueDockName="ช่องที่ 4" },
                     new QueueDock(){ QueueDockName="ช่องที่ 5" },
                     new QueueDock(){ QueueDockName="ช่องที่ 6" },
                     new QueueDock(){ QueueDockName="ช่องที่ 7" },
                     new QueueDock(){ QueueDockName="ช่องที่ 8" },
                };
        try
        {
            if (DITS.HILI.WMS.ClientService.Common.User == null)
            {

                List<QueueReg> data = CreateDummy(docks).ToList();
                var json = Json(data, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            else
            {
                var docksRes = DITS.HILI.WMS.ClientService.Queue.QueueClient.GetDockAll(null, "").Result;
                docks = docksRes.Get<List<QueueDock>>();
                int total = 0;
                QueueStatusEnum status = QueueStatusEnum.All;
                DITS.HILI.WMS.Core.Domain.ApiResponseMessage apiResp = DITS.HILI.WMS.ClientService.Queue.QueueClient.GetInQueue((int)status, string.Empty).Result;
                List<QueueReg> data = new List<QueueReg>();
                if (apiResp.ResponseCode == "0")
                {
                    total = apiResp.Totals;
                    data = apiResp.Get<List<QueueReg>>();
                }
                if (data.Count <= 0)
                {
                    data = CreateDummy(docks).ToList();
                }
                foreach (var dock in docks)
                {
                    if (!data.Any(e => e.QueueDock == dock.QueueDockName))
                    {
                        data.Add(new QueueReg()
                        {
                            QueueDock = dock.QueueDockName,
                            QueueNo = "0000",
                            QueueStatus = "",
                            ShipFrom = "",
                            TruckRegNo = "",
                            EstimateTime = 0,
                            TimeInString = "",
                            QueueStatusID = 7
                        });
                    }
                } 
                var json = Json(data, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
        }
        catch
        {
            List<QueueReg> data = CreateDummy(docks).ToList();
            var json = Json(data, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
    }

    [HttpGet]
    [Route("GetWaitingQueue")]
    public ActionResult GetWaitingQueue()
    {
        try
        {
            if (DITS.HILI.WMS.ClientService.Common.User == null)
            {
                List<QueueReg> data = CreateDummy(0, 16).ToList();
                var json = Json(data, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            else
            {
                int total = 0;
                QueueStatusEnum status = QueueStatusEnum.All;
                DITS.HILI.WMS.Core.Domain.ApiResponseMessage apiResp = DITS.HILI.WMS.ClientService.Queue.QueueClient.GetQueueInProgress((int)status, string.Empty).Result;
                List<QueueReg> data = new List<QueueReg>();
                if (apiResp.ResponseCode == "0")
                {
                    total = apiResp.Totals;
                    data = apiResp.Get<List<QueueReg>>();
                }
                if (data.Count <= 0)
                {
                    data = CreateDummy(0, 16).ToList();
                }
                if ((data.Count % 16) != 0)
                {
                    int cicle = data.Count / 16;
                    var diff = ((cicle + 1) * 16) - data.Count;
                    for (int i = 1; i <= diff; i++)
                    {
                        data.Add(new QueueReg()
                        {
                            QueueDock = (i + data.Count).ToString(),
                            QueueNo = "XXXX",
                            QueueStatus = "",
                            ShipFrom = "",
                            TruckRegNo = "",
                            EstimateTime = 0,
                            TimeInString = "",
                            QueueStatusID = 7
                        });
                    }
                }

                var json = Json(data, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
        }
        catch
        {
            List<QueueReg> data = CreateDummy(0, 16).ToList();
            var json = Json(data, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
    }
    [HttpPost]
    [Route("RefreshQueue")]
    public ActionResult RefreshQueue(string message)
    {
        QueueHub.RefreshQueue(null);
        return new EmptyResult();
    }
    [HttpPost]
    [Route("CallQueue")]
    public ActionResult CallQueue(string QueueNo, string DockNo,string LicensePlate)
    {
        try
        {
            string input = DockNo;
            string result = Regex.Replace(input, @"[^\d]", "");
            string dockno = result;
            Console.WriteLine(dockno);
            List<string> files = new List<string>
            {
                _soundPath + "please.wav"
            };
            files.Add(_soundPath + "number.wav");
            //files.AddRange(GetFileSoundTh(QueueNo).ToArray());
            files.AddRange(GetFileSoundThByLicensePlate(LicensePlate).ToArray());
            files.Add(_soundPath + "chanel.wav");
            files.Add(_soundPath + dockno + ".wav");
            files.Add(_soundPath + "ka.wav");
            string newpath = MergeAudio(QueueNo, files); 
            QueueHub.SentCallQueue(newpath, null); 
        }
        catch
        {

        }
        return new EmptyResult();
    }

   
    [HttpPost]
    [Route("CompletedQueue")]
    public ActionResult CompletedQueue(string QueueNo, string DockNo, string LicensePlate)
    {
        try
        {
            string input = DockNo;
            string result = Regex.Replace(input, @"[^\d]", "");
            string dockno = result;
            Console.WriteLine(dockno);
            List<string> files = new List<string>
            {
                _soundPath + "please.wav"
            };
            //files.AddRange(GetFileSoundTh(QueueNo).ToArray());
            files.AddRange(GetFileSoundThByLicensePlate(LicensePlate).ToArray());
            files.Add(_soundPath + "Chanel.wav");
            files.Add(_soundPath + dockno + ".wav");
            string newpath = MergeAudio(QueueNo, files);
            //System.Media.SoundPlayer s = new System.Media.SoundPlayer
            //{
            //    SoundLocation = Server.MapPath(newpath.Replace("~/", "../"))
            //};
            //s.PlaySync();

            QueueHub.SentCompletedQueue(newpath, null);
        }
        catch
        {

        }
        return new EmptyResult();
    }

    [HttpGet]
    [Route("LoadAudio")]
    public ActionResult LoadAudio(string audio)
    {
        var newpath = string.Format("{0}/{1}", _tmpSoundPath, audio);
        return Json(newpath.Replace("../","~/")); 
    }
    [HttpPost]
    [Route("ChangedAnounce")]
    public ActionResult ChangedAnounce(string Message, bool EnableMessage)
    {
        QueueHub.ChangedAnounce(Message, EnableMessage, null); 
        return new EmptyResult();
    }
    private string MergeAudio(string QueueNo, List<string> files)
    {
        try
        { 
            var newWav = Concatenate(files);
            var newpath = string.Format("{0}/{1}", _tmpSoundPath, string.Format("Q{0}.wav", QueueNo));
            if (System.IO.File.Exists(Server.MapPath(newpath)))
            {
                System.IO.File.Delete(Server.MapPath(newpath));
                System.Threading.Thread.Sleep(500);
            }
            System.IO.File.WriteAllBytes(Server.MapPath(newpath), newWav);
            return newpath.Replace("../", "~/");
        }
        catch//(Exception ex)
        {
            return "";
        }
    } 
    public byte[] Concatenate(IEnumerable<string> sourceData)
    {
        var buffer = new byte[1024 * sourceData.Count()];
        WaveFileWriter waveFileWriter = null;
        using (var output = new MemoryStream())
        {
            try
            {
                foreach (var item in sourceData)
                {

                    if (System.IO.File.Exists(Server.MapPath(item)))
                    {
                        var binaryData = System.IO.File.ReadAllBytes(Server.MapPath(item));

                        using (var audioStream = new MemoryStream(binaryData))
                        {
                            using (WaveFileReader reader = new WaveFileReader(audioStream))
                            {
                                if (waveFileWriter == null)
                                {
                                    waveFileWriter = new WaveFileWriter(output, reader.WaveFormat);
                                }
                                if (!AssertWaveFormat(reader, waveFileWriter))
                                {
                                    var byts = Encode(Server.MapPath(item), reader.WaveFormat);
                                    var audioStream2 = new MemoryStream(byts);
                                    WaveFileReader reader2 = new WaveFileReader(audioStream2);
                                    WaveStreamWrite(reader2, waveFileWriter, buffer);
                                }
                                else
                                {
                                    WaveStreamWrite(reader, waveFileWriter, buffer);
                                } 
                            }
                        }
                    }
                    waveFileWriter.Flush();
                }
                return output.ToArray();
            }
            catch(Exception ex)
            {
                waveFileWriter?.Dispose();
                return buffer;
            }
            finally
            {
                waveFileWriter?.Dispose();
            }
        }
    }
    private byte[] Encode(string filename, WaveFormat waveFormat)
    { 
        try
        {
            var bytes = System.IO.File.ReadAllBytes(filename);
            using (var audioStream = new MemoryStream(bytes))
            {
                using (var reader = new WaveFileReader(audioStream))
                {
                    var newFormat = new WaveFormat(waveFormat.SampleRate, waveFormat.BitsPerSample, waveFormat.Channels);
                    using (var conversionStream = new WaveFormatConversionStream(newFormat, reader))
                    {
                        WaveFileWriter.CreateWaveFile(filename, conversionStream);
                    }
                }
            }
        }catch(Exception ex)
        {

        }
        return System.IO.File.ReadAllBytes(filename);
    }
    private bool AssertWaveFormat(WaveFileReader reader, WaveFileWriter writer)
    {
        return reader.WaveFormat.Equals(writer.WaveFormat); 
    }
    private void WaveStreamWrite(WaveFileReader reader, WaveFileWriter writer, byte[] buffer)
    {
        int read;
        while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
        {
            writer.Write(buffer, 0, read);
        }
    }
    private List<string> GetFileSoundThByLicensePlate(string licensePlate)
    {
        string newPlate = licensePlate.Replace("-", "").Replace(" ", "").TrimEnd().TrimStart().Trim();
        char[] array = newPlate.ToCharArray();
        List<string> files = new List<string>();
        foreach (var item in array)
        {
            files.Add(string.Format(_soundPath + "{0}.wav", item));
        }
        return files;
    }

    private List<string> GetFileSoundTh(string queuno)
    {
        List<string> files = new List<string>(); 
        int.TryParse(queuno, out int queuint);
        if (queuint > 99)
        {
            int hundred = (queuint / 100);
            files.Add(_soundPath + hundred.ToString() + ".wav");
            files.Add(_soundPath + "100.wav");
            queuint = queuint - (hundred * 100);
        }
        if (queuint > 9)
        {
            string qstr = queuint.ToString();
            string d1 = qstr.Substring(0, 1);
            string d2 = qstr.Substring(1, 1);
            if (d1 == "1")
            {
                files.Add(_soundPath + "10.wav");
            }
            else
            {
                if (d1 == "2")
                {
                    files.Add(_soundPath + "20.wav");
                }
                else
                {
                    files.Add(_soundPath + d1 + ".wav");
                    files.Add(_soundPath + "10.wav");
                }
            }
            if (d2 == "1")
            {
                files.Add(_soundPath + "ed.wav");
            }
            else if (d2 != "0")
            {
                files.Add(_soundPath + d2 + ".wav");
            }
        }
        else
        {
            files.Add(_soundPath + queuint.ToString() + ".wav");
        } 
        return files;
    }
     
    private IEnumerable<QueueReg> CreateDummy(List<QueueDock> docks)
    {
        foreach (var dock in docks)
        {
            yield return new QueueReg()
            {
                QueueDock = dock.QueueDockName,
                QueueNo = "",
                QueueStatus = "",
                ShipFrom = "",
                TruckRegNo = "",
                EstimateTime = 0,
                TimeInString = "",
                QueueStatusID = 7 
            };
        }
    }
    private IEnumerable<QueueReg> CreateDummy(int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            yield return new QueueReg()
            {
                QueueDock = "ช่องที่ " + (i).ToString(),
                QueueNo = "XXXX",
                QueueStatus = "",
                ShipFrom = "",
                TruckRegNo = "",
                EstimateTime = 0,
            };
        }
    } 

    private void AutoLogon()
    {
        try
        {
            //if (DITS.HILI.WMS.ClientService.Common.User != null)
            //{
            //    return;
            //}
            string username = ConfigurationManager.AppSettings["QueueUser"].ToString();
            string password = ConfigurationManager.AppSettings["QueuePassword"].ToString();
            DITS.HILI.WMS.Core.Domain.ApiResponseMessage api = DITS.HILI.WMS.ClientService.WMSProperty.GetToken(username, password).Result;
            if (api.IsSuccess)
            {
                DITS.HILI.WMS.ClientService.Common.AccessToken = (Token)api.Data;
                DITS.HILI.WMS.Core.Domain.ApiResponseMessage apiResp = DITS.HILI.WMS.ClientService.WMSProperty.GetUser(username).Result;
                if (apiResp.IsSuccess)
                {
                    UserAccounts user = apiResp.Get<UserAccounts>();
                    if (user != null)
                    {
                        DITS.HILI.WMS.ClientService.Common.User = user;
                    }
                }
            }
        }
        catch
        {
            // throw ex;
        }
    }
}
