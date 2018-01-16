using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using DemoService.Data;

namespace DemoService.Controllers
{
    /// <summary>
    /// base controller
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// used for processing functions on data
        /// </summary>
        public IDataProcessor DataProcessor { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="dataProcessor">the processors to use when performing actions with data</param>
        public BaseController(IDataProcessor dataProcessor)
        {
            DataProcessor = dataProcessor;
        }
    }
}
