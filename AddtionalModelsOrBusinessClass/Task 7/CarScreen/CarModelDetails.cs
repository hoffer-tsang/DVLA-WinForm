/*==============================================================================
 *
 * Car Model Class
 *
 * Copyright © Dorset Software Services Ltd, 2022
 *
 * TSD Section: P770 DataBase Driven Application Task Set 3 Task 7
 *
 *============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameWorkModel;

namespace AddtionalModelsOrBusinessClass.Task_7.CarScreen
{
    public class CarModelDetails
    {
        public int CarId { get; set; }
        public string RegistrationNumber { get; set; }
        public string Colour { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public System.DateTime RegistrationDate { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public ICollection<Sighting> Sightings { get; set; }
        /// <summary>
        /// get the car details to be display in car details screen
        /// </summary>
        /// <param name="registrationNumber"> the registration number to be display </param>
        /// <returns> car details </returns>
        public CarModelDetails CarDetailsToDisplay(string registrationNumber)
        {
            using (var context = new DVLAEntities())
            {
                var car = context.Cars.Select(
                c => new CarModelDetails
                {
                    CarId = c.CarId,
                    RegistrationNumber = c.RegistrationNumber,
                    Model = c.Model.Name,
                    Make = c.Model.Make.Name,
                    OwnerFirstName = c.Owner.FirstName,
                    OwnerLastName = c.Owner.LastName,
                    Colour = c.Colour.Name,
                    RegistrationDate = c.RegistrationDate,
                    Sightings = c.Sightings,
                }).Where(c => c.RegistrationNumber == registrationNumber).ToList();
                return car[0];
            }
        }
        /// <summary>
        /// save the new car detials or updated car details in database
        /// </summary>
        /// <param name="input"> input new car or update detials </param>
        /// <param name="registrationNumber"> registration number of car </param>
        /// <param name="model"> model of car </param>
        /// <param name="colour"> colour of car </param>
        /// <param name="registrationDate"> registration date of car </param>
        /// <param name="firstName"> first name of owner </param>
        /// <param name="lastName"> last name of owner </param>
        /// <param name="carId"> car ID if update car, if not -1 </param>
        /// <returns> 1 if owner is found, return -1 if owner does not exists </returns>
        public int SaveCarDetails(bool input, string registrationNumber,
            string model, string colour, DateTime registrationDate, string firstName, string lastName, int carId)
        {
            int modelId = GetModelId(model);
            int colourId = GetColourId(colour);
            int ownerId = GetOwnerId(firstName, lastName);
            if (ownerId == -1)
            {
                return -1;
            }
            using (var context = new DVLAEntities())
            {
                Car car = new Car();
                if (input == false)
                {
                    car = context.Cars.Find(carId);
                }
                else
                {
                    car = new Car();
                    context.Cars.Add(car);
                }
                car.RegistrationNumber = registrationNumber;
                car.ModelId = modelId;
                car.ColourId = colourId;
                car.RegistrationDate = registrationDate;
                car.OwnerId = ownerId;
                context.SaveChanges();
            }
            return 1;
        }
        /// <summary>
        /// get the colour id from colour
        /// </summary>
        /// <param name="colour"> colour of car</param>
        /// <returns> colour id </returns>
        private int GetColourId(string colour)
        {
            using (var context = new DVLAEntities())
            {
                var colours = context.Colours.Select(
                c => new AddtionalModelsOrBusinessClass.Data.Colour
                {
                    ColourId = c.ColourId,
                    Name = c.Name,
                }).Where(c => c.Name == colour).ToList();
                return colours[0].ColourId;
            }
        }
        /// <summary>
        /// get model id form the model
        /// </summary>
        /// <param name="model"> model of car </param>
        /// <returns> model id </returns>
        private int GetModelId(string model)
        {
            using (var context = new DVLAEntities())
            {
                var models = context.Models.Select(
                m => new AddtionalModelsOrBusinessClass.Data.Model
                {
                    ModelId = m.ModelId,
                    Name = m.Name,
                }).Where(m => m.Name == model).ToList();
                return models[0].ModelId;
            }
        }
        /// <summary>
        /// get owner id from first name and last name
        /// </summary>
        /// <param name="firstName"> first name of owner </param>
        /// <param name="lastName"> last name of owner </param>
        /// <returns> owner id </returns>
        private int GetOwnerId(string firstName, string lastName)
        {
            using (var context = new DVLAEntities())
            {
                var owners = context.Owners.Select(
                o => new AddtionalModelsOrBusinessClass.Data.Owner
                {
                    OwnerId = o.OwnerId,
                    FirstName = o.FirstName,
                    LastName = o.LastName,
                }).Where(o => o.FirstName == firstName && o.LastName == lastName).ToList();
                if (owners.Count() == 0)
                {
                    return -1;
                }
                return owners[0].OwnerId;
            }
        }
    }
}
