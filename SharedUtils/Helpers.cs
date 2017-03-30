using System;
using System.Reflection;

namespace SharedUtils
{
    public static class Helpers
    {
        /// <summary>
        /// Get PropertyValue of source object
        /// </summary>
        /// <param name="src">Object source</param>
        /// <param name="propName">Property's name</param>
        /// <returns>Property's value or null if something went wrong</returns>
        public static object GetPropValue(object src, string propName)
        {
            //Check if source object exists and property's name valid.
            if (src == null || string.IsNullOrWhiteSpace(propName)) return null;
            PropertyInfo PropInfo = src.GetType().GetProperty(propName);
            //Check if property exists in source object.
            if (PropInfo == null) return null;
            return PropInfo.GetValue(src, null);
        }

        /// <summary>
        /// Assign a value to specified property in an object
        /// </summary>
        /// <typeparam name="T">Type of Property</typeparam>
        /// <param name="src">Object source</param>
        /// <param name="propName">Property's name</param>
        /// <param name="value">Property's new value</param>
        /// <returns>true if succeeded</returns>
        public static bool SetPropValue<T>(object src, string propName, T value)
        {
            //Check if source object exists and property's name valid.
            if (src == null || string.IsNullOrWhiteSpace(propName)) return false;
            PropertyInfo PropInfo = src.GetType().GetProperty(propName);
            //Check if property exists in source object.
            if (PropInfo == null) return false;
            //Check if property's type matches our new value type
            if (PropInfo.PropertyType != typeof(T)) return false;
            PropInfo.SetValue(src, value);
            //Success
            return true;
        }

        /// <summary>
        /// Returns pluralize form depending of amount.
        /// </summary>
        /// <param name="amount">Quantity</param>
        /// <param name="LangCode">Language in 2 letters LowerCase style (for now can be en, fr or pl)</param>
        /// <param name="PluralForm">string possibilities</param>
        /// <returns>Pluralized string or string error</returns>
        public static string Pluralize(int amount, string LangCode, params string[] PluralForm)
        {
            //<see>http://localization-guide.readthedocs.org/en/latest/l10n/pluralforms.html</see>
            switch (LangCode)
            {
                case "pl":
                    //nplurals=3; plural=(n==1 ? 0 : n%10>=2 && n%10<=4 && (n%100<10 || n%100>=20) ? 1 : 2);
                    if (amount <= 1) return PluralForm.Length >= 1 ? PluralForm[0] : "PluralForm Index 0 Not Found";
                    if (amount % 10 >= 2 && amount % 10 <= 4 && (amount % 100 < 10 || amount % 100 >= 20)) return PluralForm.Length >= 2 ? PluralForm[1] : "PluralForm Index 1 Not Found";
                    return PluralForm.Length >= 3 ? PluralForm[2] : "PluralForm Index 2 Not Found";
                case "en":
                case "fr":
                    if (amount <= 1) return PluralForm.Length >= 1 ? PluralForm[0] : "PluralForm Index 0 Not Found";
                    else return PluralForm.Length >= 2 ? PluralForm[1] : "PluralForm Index 1 Not Found";
            }
            return string.Empty;
        }

        /// <summary>
        /// Transform given datetime to string using language & timezone
        /// </summary>
        /// <param name="sourceDateTime">source date in UTC format</param>
        /// <param name="TimeZone">destination timezone</param>
        /// <param name="LangCode">Language in 2 letters LowerCase style (for now can be en, fr or pl)</param>
        /// <returns>string formatted datetime</returns>
        public static string PrettifyDate(DateTime sourceDateTime, TimeZoneInfo TimeZone, string LangCode)
        {
            TimeSpan span = DateTime.UtcNow - sourceDateTime;
            if (span.Days > 0)
            {
                var nDate = TimeZoneInfo.ConvertTime(sourceDateTime, TimeZone);
                switch (LangCode)
                {
                    case "fr":
                        return string.Format("Le {0:dd/MM/yyyy à HH:mm}", nDate);
                    case "pl":
                        return string.Format("{0:dd/MM/yyyy o HH:mm}", nDate);
                }
                return string.Format("{0:yyyy/MM/dd} at {0:hh:mm tt}", nDate);
            }
            if (span.Hours > 0)
            {
                switch (LangCode)
                {
                    case "fr":
                        return string.Format("Il y a {0} heure{1} et {2} minute{3}",
                            span.Hours,
                            span.Hours > 1 ? "s" : string.Empty,
                            span.Minutes,
                            span.Minutes > 1 ? "s" : string.Empty);
                    case "pl":
                        return string.Format("{0} {1} i {2} {3} temu",
                            span.Hours,
                            Pluralize(span.Hours, "pl", "godzinę", "godziny", "godzin"),
                            span.Minutes,
                            Pluralize(span.Minutes, "pl", "minutę", "minuty", "minut"));
                }
                return string.Format("{0} hour{1} and {2} minute{3} ago",
                    span.Hours,
                    span.Hours > 1 ? "s" : string.Empty,
                    span.Minutes,
                    span.Minutes > 1 ? "s" : string.Empty);
            }
            if (span.Minutes > 0)
            {
                switch (LangCode)
                {
                    case "fr":
                        return string.Format("Il y a {0} minute{1}",
                            span.Minutes,
                            span.Minutes > 1 ? "s" : string.Empty);
                    case "pl":
                        return string.Format("{0} {1} temu",
                            span.Minutes,
                            Pluralize(span.Minutes, "pl", "minutę", "minuty", "minut"));
                }
                return string.Format("{0} minute{1} ago",
                    span.Minutes,
                    span.Minutes > 1 ? "s" : string.Empty);
            }
            switch (LangCode)
            {
                case "fr":
                    return "Il y a moins d'une minute";
                case "pl":
                    return "Mniej niż minutę temu";
            }
            return "Less than a minute ago";
        }
    }
}
