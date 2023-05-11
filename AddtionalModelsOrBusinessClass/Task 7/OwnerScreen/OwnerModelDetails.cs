/*==============================================================================
 *
 * Owner Details Class
 *
 * Copyright © Dorset Software Services Ltd, 2022
 *
 * TSD Section: P770 DataBase Driven Application Task Set 3 Task 7
 *
 *============================================================================*/
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameWorkModel;

namespace AddtionalModelsOrBusinessClass.Task_7.OwnerScreen
{
    public class OwnerModelDetails
    {
        public int OwnerId { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public byte[] RowVersion { get; set; }
        public ICollection<Car> CarList { get; set; }
        /// <summary>
        /// Display Owner Details in Owner Details Screen
        /// </summary>
        /// <param name="displayList"> the details in owner screen display list </param>
        /// <returns> the specifc owner model details </returns>
        public OwnerModelDetails OwnerDetailsToDisplay(OwnerSearchDisplayList displayList)
        {
            using (var context = new DVLAEntities())
            {
                var owner = context.Owners.Select(
                o => new OwnerModelDetails
                {
                    OwnerId = o.OwnerId,
                    FirstName = o.FirstName,
                    LastName = o.LastName,
                    DateOfBirth = o.DateOfBirth,
                    Line1 = o.Address.Line1,
                    Line2 = o.Address.Line2,
                    Line3 = o.Address.Line3,
                    City = o.Address.City,
                    County = o.Address.County,
                    Country = o.Address.Country,
                    PostCode = o.Address.PostalCode,
                    CarList = o.Cars,
                    RowVersion = o.RowVersion,
                }).Where(o => o.FirstName == displayList.FirstName && 
                o.LastName == displayList.LastName &&
                o.DateOfBirth == displayList.DateOfBirth).ToList();
                if (owner.Count == 0)
                {
                    return null;
                }
                else
                {
                    return owner[0];
                }
            }
        }
        /// <summary>
        /// save the new car detials or updated car details in database
        /// </summary>
        /// <param name="input"> input new owner or update detials </param>
        /// <param name="dateOfBirth"> date of birth of owner </param>
        /// <param name="firstName"> first name of owner</param>
        /// <param name="lastName"> last name of owner</param>
        /// <param name="line1"> line1 of owner address </param>
        /// <param name="line2"> line2 of owner address </param>
        /// <param name="line3"> line3 of owner address </param>
        /// <param name="city"> city of owner address </param>
        /// <param name="county"> county of owner address </param>
        /// <param name="country"> country of owner address </param>
        /// <param name="postcode"> postcode of owner address </param>
        /// <param name="ownerId"> ownerId to be update in details </param>
        /// <param name="rowVersion"> the row version of the owner for concurrency check </param>
        /// <returns> return 1 if save success, or -1 for invalid address detials,
        /// or -2 for concurrency exceptions </returns>
        public int SaveOwnerDetails(bool input, DateTime dateOfBirth,
            string firstName, string lastName, string line1, string line2,
            string line3, string city, string county,
            string country, string postcode, int ownerId, byte[] rowVersion)
        {
            int addressId = GetAddressId(line1, line2,
            line3, city, county, country, postcode);
            if (addressId == -1)
            {
                return -1;
            }
            using (var context = new DVLAEntities())
            {
                var owner = new EntityFrameWorkModel.Owner();
                if (input == false)
                {
                    owner = context.Owners.Find(ownerId);
                    context.Entry(owner).Property(o => o.RowVersion).OriginalValue = rowVersion;
                }
                else
                {
                    owner = new EntityFrameWorkModel.Owner();
                    context.Owners.Add(owner);
                }
                owner.FirstName = firstName;
                owner.LastName = lastName;
                owner.DateOfBirth = dateOfBirth;
                owner.AddressId = addressId;
                try
                {
                    context.SaveChanges();
                }
                catch(DbUpdateConcurrencyException)
                {
                    return -2;
                }
            }
            return 1;
        }
        /// <summary>
        /// Find the address Id from address details 
        /// </summary>
        /// <param name="line1"> line1 of owner address </param>
        /// <param name="line2"> line2 of owner address </param>
        /// <param name="line3"> line3 of owner address </param>
        /// <param name="city"> city of owner address </param>
        /// <param name="county"> county of owner address </param>
        /// <param name="country"> country of owner address </param>
        /// <param name="postcode"> postcode of owner address </param>
        /// <returns> address id, or -1 if fail to found address id </returns>
        private int GetAddressId(string line1, string line2,
            string line3, string city, string county,
            string country, string postcode)
        {
            using (var context = new DVLAEntities())
            {
                var addresses = context.Addresses.Select(
                a => new AddtionalModelsOrBusinessClass.Data.Address
                {
                    AddressId = a.AddressId,
                    Line1 = a.Line1,
                    Line2 = a.Line2,
                    Line3 = a.Line3,
                    City = a.City,
                    County = a.County,
                    Country = a.Country,
                    PostCode = a.PostalCode,

                }).Where(a => (a.Line1 == line1 && a.City == city && a.County == county && a.Country == country && a.PostCode == postcode));
                if (!string.IsNullOrWhiteSpace(line2))
                {
                    addresses = addresses.Where(a => a.Line2 == line2);
                }
                if(!string.IsNullOrWhiteSpace(line3))
                {
                    addresses = addresses.Where(a => a.Line3 == line3);
                }
                if (addresses.ToList().Count() == 0)
                {
                    return -1;
                }
                return addresses.ToList()[0].AddressId;
            }
        }
    }
}
