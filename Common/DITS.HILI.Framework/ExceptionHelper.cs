using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace DITS.HILI.Framework
{
    public static class ExceptionHelper
    {
        public static Exception ExceptionMessage(DbEntityValidationException ex)
        {
            IEnumerable<string> errorMessage = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
            string fullErrorMessage = string.Join("; ", errorMessage);
            string exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
            DbEntityValidationException e = new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            return new Exception(e.Message);
        }

        public static Exception ExceptionMessage(this Exception ex)
        {
            IEnumerable<string> messages = ex.FromHierarchy(e => e.InnerException)
                .Select(e => e.Message);
            return new Exception(string.Join(Environment.NewLine, messages));
        }

        private static IEnumerable<TSource> FromHierarchy<TSource>(
           this TSource source,
           Func<TSource, TSource> nextItem)
           where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        private static IEnumerable<TSource> FromHierarchy<TSource>(
                     this TSource source,
                     Func<TSource, TSource> nextItem,
                     Func<TSource, bool> canContinue)
        {
            for (TSource current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }
    }
}
