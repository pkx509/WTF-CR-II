using System;

namespace DITS.HILI.WMS.Web.Common.UI
{
    public class ValidateDateInput
    {
        /// <summary>
        /// return true = Validate Pass , false = Validate Not-Pass
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static bool Validate_Start_End_Date(DateTime? startDate, DateTime? endDate, out string errorMsg)
        {
            errorMsg = "no Error ;-)";
            bool result = true;

            if (startDate == null || endDate == null)
            {
                errorMsg = "Start/End Date shouldn't be null";
                result = false;
            }

            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
            {
                errorMsg = "Start/End Date shouldn't be empty";
                result = false;
            }

            if (endDate.Value < startDate.Value)
            {
                errorMsg = "EndDate should greater than StartDate";
                result = false;
            }

            return result;
        }
    }
}