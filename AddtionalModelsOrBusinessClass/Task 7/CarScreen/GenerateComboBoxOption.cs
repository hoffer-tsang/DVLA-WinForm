/*==============================================================================
 *
 * Combo Box Class
 *
 * Copyright © Dorset Software Services Ltd, 2022
 *
 * TSD Section: P770 DataBase Driven Application Task Set 3 Task 7
 *
 *============================================================================*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameWorkModel;

namespace AddtionalModelsOrBusinessClass.Task_7.CarScreen
{
    /// <summary>
    /// Generate Combo Box drop down option
    /// </summary>
    public class GenerateComboBoxOption
    {
        /// <summary>
        /// Get all model in database 
        /// </summary>
        /// <returns> a list of model </returns>
        public List<string> AvaliableModel()
        {
            using (var context = new DVLAEntities())
            {
                var models = context.Models.Select(
                    m => new CarSearch
                    {
                        Model = m.Name,
                    }).ToList();
                List<string> modelList = new List<string>();
                foreach (CarSearch model in models)
                {
                    modelList.Add(model.Model);
                }
                return modelList;
            }
        }
        /// <summary>
        /// Get all Make in database
        /// </summary>
        /// <returns> a list of make </returns>
        public List<string> AvaliableMake()
        {
            using (var context = new DVLAEntities())
            {
                var makes = context.Makes.Select(
                    m => new CarSearch
                    {
                        Make = m.Name,
                    }).ToList();
                List<string> makeList = new List<string>();
                foreach (CarSearch make in makes)
                {
                    makeList.Add(make.Make);
                }
                return makeList;
            }
        }
        /// <summary>
        /// get all avaliable colour in database
        /// </summary>
        /// <returns> a list of colour </returns>
        public List<string> AvaliableColour()
        {
            using (var context = new DVLAEntities())
            {
                var colours = context.Colours.Select(
                    c => new CarModelDetails
                    {
                        Colour = c.Name
                    }).ToList();
                List<string> colourList = new List<string>();
                foreach (CarModelDetails colour in colours)
                {
                    colourList.Add(colour.Colour);
                }
                return colourList;
            }
        }
        /// <summary>
        /// get the corresponing model value for make value for update
        /// </summary>
        /// <param name="make"> make value of car </param>
        /// <returns> model value of the make</returns>
        public List<string> ModelValueUpdate(string make)
        {
            
            using (var context = new DVLAEntities())
            {
                var models = context.Models.Select(
                    m => new CarSearch
                    {
                        Model = m.Name,
                        Make = m.Make.Name,
                    }).Where(m => m.Make == make).ToList();
                var modelList = new List<string>();
                foreach(var model in models)
                {
                    modelList.Add(model.Model);
                }
                return modelList;
            }
        }
        /// <summary>
        /// Update the make List avaliable, after model is chosen
        /// </summary>
        /// <param name="model"> model chosen </param>
        /// <param name="isModelChanged"> is the model chosen </param>
        /// <returns> list of make of the model to display, 
        /// if model is not changed return null </returns>
        public List<string> MakeValueUpdate(string model, bool isModelChanged)
        {
            if(isModelChanged == true)
            {
                return null;
            }
            else
            {
                List<string> makeList = new List<string>();
                using (var context = new DVLAEntities())
                {
                    var make = context.Models.Select(
                        m => new CarSearch
                        {
                            Model = m.Name,
                            Make = m.Make.Name,
                        }).Where(m => m.Model == model).ToList();
                    makeList.Add(make[0].Make);
                    return makeList;
                }
            }
        }
    }
}
