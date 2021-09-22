using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Utility
{

    public static class Error
    {
        public static List<string> GetEntityErrors(Exception e)
        {
            List<string> errList = new List<string>();

            DbEntityValidationException ex = e is DbEntityValidationException ?
                e as DbEntityValidationException : null;

            if (ex == null) { return errList; }

            foreach (var eve in ex.EntityValidationErrors)
            {
                var Errors = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);

                foreach (var ve in eve.ValidationErrors)
                {
                    errList.Add(string.Format("- Field: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                }
            }

            return errList;
        }

        public static string GetEntityErrorsAsSting(Exception e)
        {
            List<string> errList = new List<string>();

            DbEntityValidationException ex = e is DbEntityValidationException ?
                e as DbEntityValidationException : null;

            if (ex == null) { return string.Empty; }

            StringBuilder sbErrors = new StringBuilder();

            foreach (var eve in ex.EntityValidationErrors)
            {
                var Errors = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);

                foreach (var ve in eve.ValidationErrors)
                {
                    sbErrors.AppendLine(string.Format("- Field: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                }
            }

            return sbErrors.ToString();
        }
    }

    public static class DefaultList
    {
        public static List<SelectListItem> GetUnitList()
        {
            List<SelectListItem> unitList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="No.", Value="No."},
                new SelectListItem() {Text="KW", Value="KW"},
            };

            return unitList;
        }
    }
}
