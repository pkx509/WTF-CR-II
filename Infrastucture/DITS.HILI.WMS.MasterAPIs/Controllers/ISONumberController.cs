using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.Core.ServiceAPIs;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterService.Utility;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.MasterAPIs.Controllers
{
    [Authorize]
    public class ISONumberController : BaseApiController
    {
        private readonly IISONumberService _ISOService;
        private readonly IPrinterService _Service;
        private readonly IMessageService _messageService;

        public ISONumberController(IPrinterService service, IISONumberService ISOservice, IMessageService messageService)
        {
            _ISOService = ISOservice;
            _Service = service;
            _messageService = messageService;
        }

        //[HttpGet]
        //[Route("api/printer/getbyid")]
        //// [ResponseType(typeof(Units))]
        //public async Task<ApiResponseMessage> getbyid(Guid id)
        //{
        //    try
        //    {
        //        int _totalRecord = 0;
        //        var _header = ApiHelpers.Response(Request);
        //        var result = await Task.Run(() =>
        //        {
        //            return _Service.GetById(id);
        //        });

        //        return Succeed(result, _totalRecord);
        //    }
        //    catch (HILIException ex)
        //    {
        //        return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, this.Language));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", this.Language));
        //    }
        //}


        //[HttpGet]
        //[Route("api/printer/get")]
        ////[ResponseType(typeof(List<Units>))]
        //public async Task<ApiResponseMessage> Get(Guid? lineID, string keyword, int pageIndex = 0, int pageSize = 0)
        //{
        //    try
        //    {
        //        int _totalRecord = 0;
        //        var _header = ApiHelpers.Response(Request);
        //        var result = await Task.Run(() =>
        //        {
        //            return _Service.Get(lineID, keyword, out _totalRecord, pageIndex, pageSize);
        //        });

        //        return Succeed(result, _totalRecord);
        //    }
        //    catch (HILIException ex)
        //    {
        //        return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, this.Language));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", this.Language));
        //    }
        //}

        //[HttpGet]
        //[Route("api/printer/getprintermachine")]
        ////[ResponseType(typeof(List<Units>))]
        //public async Task<ApiResponseMessage> GetPrinterMachine(string keyword, int pageIndex = 0, int pageSize = 0)
        //{
        //    try
        //    {
        //        int _totalRecord = 0;
        //        var _header = ApiHelpers.Response(Request);
        //        var result = await Task.Run(() =>
        //        {
        //            return _Service.GetPrinterMachine(keyword, out _totalRecord, pageIndex, pageSize);
        //        });

        //        return Succeed(result, _totalRecord);
        //    }
        //    catch (HILIException ex)
        //    {
        //        return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, this.Language));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", this.Language));
        //    }
        //}

        //[HttpGet]
        //[Route("api/printer/getprinterlocation")]
        ////[ResponseType(typeof(List<Units>))]
        //public async Task<ApiResponseMessage> GetPrinterLocation(string keyword, int pageIndex = 0, int pageSize = 0)
        //{
        //    try
        //    {
        //        int _totalRecord = 0;
        //        var _header = ApiHelpers.Response(Request);
        //        var result = await Task.Run(() =>
        //        {
        //            return _Service.GetPrinterLocation(keyword, out _totalRecord, pageIndex, pageSize);
        //        });

        //        return Succeed(result, _totalRecord);
        //    }
        //    catch (HILIException ex)
        //    {
        //        return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, this.Language));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", this.Language));
        //    }
        //}



        //[HttpPost]
        //[Route("api/printer/add")]
        //// [ResponseType(typeof(Units))]
        //public async Task<ApiResponseMessage> add(Printer entity)
        //{
        //    try
        //    {
        //        var _header = ApiHelpers.Response(Request);
        //        await Task.Run(() =>
        //        {
        //            _Service.AddPrinter(entity);
        //        });

        //        return Succeed();
        //    }
        //    catch (HILIException ex)
        //    {
        //        return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, this.Language));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", this.Language));
        //    }
        //}

        //[HttpPut]
        //[Route("api/printer/modify")]
        //public async Task<ApiResponseMessage> modify(Printer entity)
        //{
        //    try
        //    {
        //        await Task.Run(() =>
        //        {
        //            _Service.ModifyPrinter(entity);
        //        });

        //        return Succeed();
        //    }
        //    catch (HILIException ex)
        //    {
        //        return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, this.Language));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", this.Language));
        //    }
        //}

        //[HttpDelete]
        //[Route("api/printer/remove")]
        //public async Task<ApiResponseMessage> remove(Guid id)
        //{
        //    try
        //    {
        //        var _header = ApiHelpers.Response(Request);
        //        await Task.Run(() =>
        //        {
        //            _Service.RemovePrinter(id);
        //        });

        //        return Succeed();
        //    }
        //    catch (HILIException ex)
        //    {
        //        return Error(ApiResponseCode.OK, _messageService.GetMessage(ex.ErrorCode, this.Language));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ApiResponseCode.InternalServerError, _messageService.GetMessage("SYS99999", this.Language));
        //    }
        //}

        [HttpGet]
        [Route("api/IsoNumber/getISONumberAll")]
        public async Task<ApiResponseMessage> getISONumberAll(Guid? IsoId, string keyword, bool Active, int? pageIndex, int? pageSize)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                System.Collections.Generic.List<ISONumber> result = await Task.Run(() =>
                {
                    return _ISOService.GetISONumber(IsoId, keyword, Active, out _totalRecord, pageIndex, pageSize);
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
        [Route("api/IsoNumber/GetISONumberByID")]
        public async Task<ApiResponseMessage> GetISONumberpByID(Guid id)
        {
            try
            {
                int _totalRecord = 0;
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                ISONumber result = await Task.Run(() =>
                {
                    return _ISOService.GetIsoByID(id);
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


        [HttpPost]
        [Route("api/IsoNumber/add")]
        public async Task<ApiResponseMessage> add(ISONumber entity)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _ISOService.AddISONumber(entity);
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

        [HttpPut]
        [Route("api/IsoNumber/modify")]
        public async Task<ApiResponseMessage> modify(ISONumber entity)
        {
            try
            {
                await Task.Run(() =>
                {
                    _ISOService.ModifyISONumber(entity);
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

        [HttpDelete]
        [Route("api/IsoNumber/remove")]
        public async Task<ApiResponseMessage> removeISONumberr(Guid id)
        {
            try
            {
                HttpCustomHeaders _header = ApiHelpers.Response(Request);
                await Task.Run(() =>
                {
                    _ISOService.RemoveISONumber(id);
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

    }
}