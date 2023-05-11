/*==============================================================================
 *
 * Owner Search Display List Class
 *
 * Copyright © Dorset Software Services Ltd, 2022
 *
 * TSD Section: P770 DataBase Driven Application Task Set 3 Task 7
 *
 *============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using EntityFrameWorkModel;

namespace AddtionalModelsOrBusinessClass.Task_7.OwnerScreen
{
    public class OwnerSearchDisplayList
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AddressLine1 { get; set; }
        private int _PageSize = 5;

        /// <summary>
        /// Perform the Search in database based on search field to update Owner list
        /// </summary>
        /// <param name="firstNameSearch"> firstname to search </param>
        /// <param name="lastNameSearch"> lastname to search </param>
        /// <param name="dateOfBirthIncluded"> checkbox status for date of birth </param>
        /// <param name="dateOfBirthSearch"> date of birth to search  </param>
        /// <param name="pageNumber"> page number to be display </param>
        /// <param name="columnIndex"> column Index to be sorted </param>
        /// <returns> A list of owner base on search field to display </returns>
        public List<OwnerSearchDisplayList> SearchOwnerList(
        string firstNameSearch,
        string lastNameSearch,
        bool dateOfBirthIncluded,
        DateTime dateOfBirthSearch,
        int pageNumber,
        int columnIndex)
        {
            using (var context = new DVLAEntities())
            {
                var ownerDisplayList = context.Owners.Select(
                o => new OwnerSearchDisplayList
                {
                    FirstName = o.FirstName,
                    LastName = o.LastName,
                    DateOfBirth = o.DateOfBirth,
                    AddressLine1 = o.Address.Line1,
                });
                if (!string.IsNullOrWhiteSpace(firstNameSearch))
                {
                    ownerDisplayList = ownerDisplayList.Where(o => o.FirstName == firstNameSearch);
                }
                if (!string.IsNullOrWhiteSpace(lastNameSearch))
                {
                    ownerDisplayList = ownerDisplayList.Where(o => o.LastName == lastNameSearch);
                }
                if(dateOfBirthIncluded == true)
                {
                    ownerDisplayList = ownerDisplayList.Where(o => o.DateOfBirth == dateOfBirthSearch);
                }
                switch (columnIndex)
                {
                    case 0:
                        ownerDisplayList = ownerDisplayList.OrderBy(o => o.FirstName).Skip((pageNumber - 1) * _PageSize).Take(_PageSize);
                        break;
                    case 1:
                        ownerDisplayList = ownerDisplayList.OrderBy(o => o.LastName).Skip((pageNumber - 1) * _PageSize).Take(_PageSize);
                        break;
                    case 2:
                        ownerDisplayList = ownerDisplayList.OrderBy(o => o.DateOfBirth).Skip((pageNumber - 1) * _PageSize).Take(_PageSize);
                        break;
                    case 3:
                        ownerDisplayList = ownerDisplayList.OrderBy(o => o.AddressLine1).Skip((pageNumber - 1) * _PageSize).Take(_PageSize);
                        break;
                }
                return ownerDisplayList.ToList();
            }
        }
        /// <summary>
        /// Find total page number required for owner list 
        /// </summary>
        /// <param name="firstNameSearch"> firstname to search </param>
        /// <param name="lastNameSearch"> lastname to search </param>
        /// <param name="dateOfBirthIncluded"> checkbox status for date of birth </param>
        /// <param name="dateOfBirthSearch"> date of birth to search  </param>
        /// <returns> A list of page number to display </returns>
        public List<int> TotalOwnerPageNumber(
        string firstNameSearch,
        string lastNameSearch,
        bool dateOfBirthIncluded,
        DateTime dateOfBirthSearch)
        {
            using (var context = new DVLAEntities())
            {
                int totalPageNumber;
                List<int> pageNumberList = new List<int>();
                var ownerDisplayList = context.Owners.Select(
                o => new OwnerSearchDisplayList
                {
                    FirstName = o.FirstName,
                    LastName = o.LastName,
                    DateOfBirth = o.DateOfBirth,
                    AddressLine1 = o.Address.Line1,
                });
                if (!string.IsNullOrWhiteSpace(firstNameSearch))
                {
                    ownerDisplayList = ownerDisplayList.Where(o => o.FirstName == firstNameSearch);
                }
                if (!string.IsNullOrWhiteSpace(lastNameSearch))
                {
                    ownerDisplayList = ownerDisplayList.Where(o => o.LastName == lastNameSearch);
                }
                if (dateOfBirthIncluded == true)
                {
                    ownerDisplayList = ownerDisplayList.Where(o => o.DateOfBirth == dateOfBirthSearch);
                }
                if ((ownerDisplayList.ToList().Count() / _PageSize) == 0)
                {
                    pageNumberList.Add(1);
                    return pageNumberList;
                }
                else
                {
                    int leftover = ownerDisplayList.ToList().Count() % _PageSize;
                    totalPageNumber = (ownerDisplayList.ToList().Count() / _PageSize);
                    if (leftover > 0)
                    {
                        totalPageNumber += 1;
                    }
                    for (int i = 0; i < totalPageNumber; i++)
                    {
                        pageNumberList.Add(i + 1);
                    }
                    return pageNumberList;
                }
            }
        }
    }
}
