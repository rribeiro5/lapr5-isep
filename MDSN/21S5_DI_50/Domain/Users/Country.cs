using DDDSample1.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
namespace DDDSample1.Domain.Users
{
    [Owned]
    public class Country : IValueObject{

        public string _Country {get ; private set;}

        private Country (){}
        public Country (string newCountry){
            if (String.IsNullOrEmpty(newCountry?.Trim()))
            {
                throw new BusinessRuleValidationException($"Empty input for {newCountry}",nameof(newCountry));
            }
            /*List<string> availableCountries = CountriesList();
            var country = newCountry.Trim();
            if(!availableCountries.Contains(country)) {
                throw new BusinessRuleValidationException($"Introduced input for {newCountry} is not Available or Valid Country");
            }*/
            this._Country = newCountry.Trim();
        }


        private static List<string> CountriesList(){
            List<string> cultureList = new List<string>();

            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach(CultureInfo culture in getCultureInfo){
                RegionInfo regionInfo = new RegionInfo(culture.LCID);
                string countryName = regionInfo.EnglishName;
                if(!cultureList.Contains(countryName)) cultureList.Add(countryName);    
            }

            return cultureList;
        }

    }


}    