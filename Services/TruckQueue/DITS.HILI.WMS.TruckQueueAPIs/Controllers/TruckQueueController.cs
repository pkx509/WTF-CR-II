using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.TruckQueueModel;
using DITS.HILI.WMS.TruckQueueService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DITS.HILI.WMS.TruckQueueAPIs.Controllers
{
    [Authorize]
    public class TruckQueueController : BaseApiController
    {
        private readonly IQueueService _service;
        private readonly IMessageService _messageService;
        public TruckQueueController(IQueueService service, IMessageService messageService)
        {
            _service = service;
            _messageService = messageService;
        }

        #region---Queue Type---
        [HttpGet]
        [Route("api/TruckQueue/getqueuetypeall")]
        public async Task<ApiResponseMessage> GetQueueTypeAll(int? status, string name)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                List<QueueRegisterType> result = await Task.Run(() =>
                {
                    return _service.GetRegisterTypeAll(out _totalRecord, status, name);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/TruckQueue/getqueuetypebyid")]
        public async Task<ApiResponseMessage> GetQueueTypeById(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                QueueRegisterType result = await Task.Run(() =>
                {
                    return _service.GetRegisterType(id);
                });
                return Succeed(result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TruckQueue/addqueuetype")]
        public async Task<ApiResponseMessage> AddQueueType(QueueRegisterType entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                ApiResponseMessage _result = await Task.Run(() =>
                {
                    return _service.Add(entity);
                });
                return Succeed(_result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TruckQueue/modifyqueuetype")]
        public async Task<ApiResponseMessage> ModifyQueueType(QueueRegisterType entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                ApiResponseMessage _result = await Task.Run(() =>
                {
                    return _service.Modify(entity);
                });
                return Succeed(_result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TruckQueue/removequeuetype")]
        public async Task<ApiResponseMessage> RemoveQueueType(QueueRegisterType entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _service.Remove(entity);
                });
                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        #endregion

        #region---dock---
        [HttpGet]
        [Route("api/TruckQueue/getdockAll")]
        public async Task<ApiResponseMessage> GetDockAll(int? status, string dockname)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                List<QueueDock> result = await Task.Run(() =>
                {
                    return _service.GetDockAll(out _totalRecord, status, dockname);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/TruckQueue/getdockbyId")]
        public async Task<ApiResponseMessage> GetDockById(Guid id)
        {
            try
            { 
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId; 
                QueueDock result = await Task.Run(() =>
                {
                    return _service.GetDock(id);
                }); 
                return Succeed(result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TruckQueue/adddock")]
        public async Task<ApiResponseMessage> AddDock(QueueDock entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                ApiResponseMessage _result = await Task.Run(() =>
                {
                    return _service.Add(entity);
                });
                return Succeed(_result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TruckQueue/modifydock")]
        public async Task<ApiResponseMessage> ModifyDock(QueueDock entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                ApiResponseMessage _result = await Task.Run(() =>
                {
                    return _service.Modify(entity);
                });
                return Succeed(_result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TruckQueue/removedock")]
        public async Task<ApiResponseMessage> RemoveDock(QueueDock entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _service.Remove(entity);
                });
                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        #endregion

        #region---Status---
        [HttpGet]
        [Route("api/TruckQueue/getstatusall")]
        public async Task<ApiResponseMessage> GetStatusAll(int? status, string statusname)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                List<QueueStatus> result = await Task.Run(() =>
                {
                    return _service.GetStatusAll(out _totalRecord, status, statusname);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/TruckQueue/getstatusbyId")]
        public async Task<ApiResponseMessage> GetStatusById(int id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                QueueStatus result = await Task.Run(() =>
                {
                    return _service.GetStatus((QueueStatusEnum)id);
                });
                return Succeed(result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        //[HttpPost]
        //[Route("api/TruckQueue/addqueuestatus")]
        //public async Task<ApiResponseMessage> AddQueueStatus(QueueStatus entity)
        //{
        //    try
        //    {
        //        HttpCustomHeaders _header = ApiHelpers.Response(Request);
        //        _service.UserID = UserId;
        //        ApiResponseMessage _result = await Task.Run(() =>
        //        {
        //            return _service.Add(entity);
        //        });
        //        return Succeed(_result);
        //    }
        //    catch (HILIException ex)
        //    {
        //        return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
        //    }
        //    catch (Exception)
        //    {
        //        return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
        //    }
        //}

        //[HttpPost]
        //[Route("api/TruckQueue/modifyqueuestatus")]
        //public async Task<ApiResponseMessage> ModifyQueueStatus(QueueStatus entity)
        //{
        //    try
        //    {
        //        HttpCustomHeaders _header = ApiHelpers.Response(Request);
        //        _service.UserID = UserId;
        //        ApiResponseMessage _result = await Task.Run(() =>
        //        {
        //            return _service.Modify(entity);
        //        });
        //        return Succeed(_result);
        //    }
        //    catch (HILIException ex)
        //    {
        //        return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
        //    }
        //    catch (Exception)
        //    {
        //        return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
        //    }
        //}

        //[HttpPost]
        //[Route("api/TruckQueue/removequeuestatus")]
        //public async Task<ApiResponseMessage> RemoveQueueStatus(QueueStatus entity)
        //{
        //    try
        //    {
        //        HttpCustomHeaders _header = ApiHelpers.Response(Request);
        //        await Task.Run(() =>
        //        {
        //            _service.Remove(entity);
        //        });
        //        return Succeed();
        //    }
        //    catch (HILIException ex)
        //    {
        //        return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
        //    }
        //    catch (Exception)
        //    {
        //        return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
        //    }
        //}

        #endregion

        #region---configuration---  
        [HttpGet]
        [Route("api/TruckQueue/getqueueconfigurationactive")]
        public async Task<ApiResponseMessage> GetQueueConfigurationActive()
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                QueueConfiguration result = await Task.Run(() =>
                {
                    return _service.GetQueueConfigurationActive(out _totalRecord);
                }); 
                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/TruckQueue/getqueueconfigurationAll")]
        public async Task<ApiResponseMessage> GetQueueConfigurationAll(int? status)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                List<QueueConfiguration> result = await Task.Run(() =>
                {
                    return _service.GetConfigurationAll(out _totalRecord, status);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }
        [HttpGet]
        [Route("api/TruckQueue/getqueueconfiguration")]
        public async Task<ApiResponseMessage> GetQueueConfiguration(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                QueueConfiguration result = await Task.Run(() =>
                {
                    return _service.GetConfiguration(id);
                });
                return Succeed(result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TruckQueue/addqueueconfiguration")]
        public async Task<ApiResponseMessage> AddQueueConfiguration(QueueConfiguration entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                ApiResponseMessage _result = await Task.Run(() =>
                {
                    return _service.Add(entity);
                });
                return Succeed(_result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TruckQueue/modifyqueueconfiguration")]
        public async Task<ApiResponseMessage> ModifyQueueConfiguration(QueueConfiguration entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                ApiResponseMessage _result = await Task.Run(() =>
                {
                    return _service.Modify(entity);
                });
                return Succeed(_result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TruckQueue/removequeueconfiguration")]
        public async Task<ApiResponseMessage> RemoveQueueConfiguration(QueueConfiguration entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _service.Remove(entity);
                });
                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        #endregion

        #region---Queue Registration---
        [HttpGet]
        [Route("api/TruckQueue/getsearchqueue")]
        public async Task<ApiResponseMessage> GetSearchQueue(DateTime startDate, DateTime endDate, string status = "", string keyword = "", int? pageIndex = null, int? pageSize = null)
        {
            try
            {
                int _totalRecord = 0;
                int st_guid = 0;
                if (!int.TryParse(status, out st_guid))
                {
                    st_guid = 0;
                }
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                List<QueueReg> result = await Task.Run(() =>
                {
                    return _service.GetSearchQueue(out _totalRecord, startDate, endDate,(QueueStatusEnum)st_guid, keyword, pageIndex, pageSize);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }
        [HttpGet]
        [Route("api/TruckQueue/getqueuelist")]
        public async Task<ApiResponseMessage> GetQueueList(string status = "", string keyword = "")
        {
            try
            {               
                int _totalRecord = 0;
                int st_guid = 0;
                if (!int.TryParse(status, out st_guid))
                {
                    st_guid = 0;
                }
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                List<QueueReg> result = await Task.Run(() =>
                {
                    return _service.GetInQueue(out _totalRecord,(QueueStatusEnum) st_guid, keyword);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/TruckQueue/getqueueinprogress")]
        public async Task<ApiResponseMessage> GetQueueInprogress(string status = "", string keyword = "")
        {
            try
            {
                int st_guid = 0;
                if (!int.TryParse(status, out st_guid))
                {
                    st_guid = 0;
                }
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                List<QueueReg> result = await Task.Run(() =>
                {
                    return _service.GetInprogress(out _totalRecord, (QueueStatusEnum)st_guid,keyword);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }
        [HttpGet]
        [Route("api/TruckQueue/getqueueall")]
        public async Task<ApiResponseMessage> GetQueueAll(string status="",string keyword = "", int? pageIndex =null, int? pageSize = null)
        {
            try
            { 
                int _totalRecord = 0;
                int st_guid = 0;
                if (!int.TryParse(status, out st_guid))
                {
                    st_guid = 0;
                }
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                List<QueueReg> result = await Task.Run(() =>
                {
                    return _service.GetQueueAll(out _totalRecord, (QueueStatusEnum)st_guid, keyword, pageIndex,pageSize);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/TruckQueue/getqueuereport")]
        public async Task<ApiResponseMessage> GetQueueReport(DateTime startDate, DateTime endDate, Guid? shippingfrom, Guid? shippingTo, string status, Guid? dockId, string keyword, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                int st_guid = 0;
                if (!int.TryParse(status, out st_guid))
                {
                    st_guid = 0;
                }
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                List<QueueReg> result = await Task.Run(() =>
                {
                    return _service.GetQueueReport(out _totalRecord,startDate,endDate,shippingfrom,shippingTo,(QueueStatusEnum)st_guid,dockId, keyword, pageIndex, pageSize);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/TruckQueue/getqueuebyId")]
        public async Task<ApiResponseMessage> GetQueueById(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                QueueReg result = await Task.Run(() =>
                {
                    return _service.GetQueue(id);
                });
                return Succeed(result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TruckQueue/changequeuestatus")]
        public async Task<ApiResponseMessage> ChangeQueueStatus(QueueReg entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                ApiResponseMessage _result = await Task.Run(() =>
                {
                    return new ApiResponseMessage() { Data = _service.ChangeQueueStatus(entity.QueueId,(QueueStatusEnum) entity.QueueStatusID), ResponseCode = "0" };
                });
                return Succeed(_result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [Route("api/TruckQueue/callqueue")]
        public async Task<ApiResponseMessage> CallQueue(QueueReg entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                ApiResponseMessage _result = await Task.Run(() =>
                {
                    return new ApiResponseMessage() { Data = _service.CallQueue(entity.QueueId), ResponseCode = "0" };
                });
                return Succeed(_result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }
        [HttpPost]
        [Route("api/TruckQueue/addqueue")]
        public async Task<ApiResponseMessage> AddQueue(QueueReg entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                ApiResponseMessage _result = await Task.Run(() =>
                {
                    return new ApiResponseMessage() { Data = _service.AddQueue(entity), ResponseCode = "0" };
                });
                return Succeed(_result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TruckQueue/modifyqueue")]
        public async Task<ApiResponseMessage> ModifyQueue(QueueReg entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                ApiResponseMessage _result = await Task.Run(() =>
                {
                    return new ApiResponseMessage() { Data = _service.ModifyQueue(entity), ResponseCode = "0" };
                });
                return Succeed(_result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TruckQueue/removequeue")]
        public async Task<ApiResponseMessage> RemoveQueue(QueueReg entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _service.RemoveQueue(entity.QueueId);
                });
                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        #endregion

        #region---ship from---
        [HttpGet]
        [Route("api/TruckQueue/getshippingfromAll")]
        public async Task<ApiResponseMessage> GetShippingFromAll(int? status, string comname, string comshortname)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                List<ShippingFrom> result = await Task.Run(() =>
                {
                    return _service.GetShippingFromAll(out _totalRecord, status, comname, comshortname);
                });

                return Succeed(result, _totalRecord);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpGet]
        [Route("api/TruckQueue/getshippingfrombyId")]
        public async Task<ApiResponseMessage> GetShippingFromById(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                ShippingFrom result = await Task.Run(() =>
                {
                    return _service.GetShippingFrom(id);
                });
                return Succeed(result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TruckQueue/addshippingfrom")]
        public async Task<ApiResponseMessage> AddShippingFrom(ShippingFrom entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                ApiResponseMessage _result = await Task.Run(() =>
                {
                    return _service.Add(entity);
                });
                return Succeed(_result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TruckQueue/modifyshippingfrom")]
        public async Task<ApiResponseMessage> ModifyShippingFrom(ShippingFrom entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                _service.UserID = UserId;
                ApiResponseMessage _result = await Task.Run(() =>
                {
                    return _service.Modify(entity);
                });
                return Succeed(_result);
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        [HttpPost]
        [Route("api/TruckQueue/removeshippingfrom")]
        public async Task<ApiResponseMessage> RemoveShippingFrom(ShippingFrom entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _service.Remove(entity);
                });
                return Succeed();
            }
            catch (HILIException ex)
            {
                return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, Language));
            }
            catch (Exception)
            {
                return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", Language));
            }
        }

        #endregion
    }
}