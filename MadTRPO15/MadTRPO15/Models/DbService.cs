using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadTRPO15.Models
{
    public class DbService
    {
        private MadTrpo15Context context;
        public MadTrpo15Context Context => context;
        public ObservableCollection<Product> Products { get; set; } = new();

        private static DbService? instance;
        public static DbService Instance
        {
            get
            {
                if(instance == null)
                    instance = new DbService();
                return instance;
            }
            
        }
        private DbService()
        {
            context =new MadTrpo15Context();
        }

       

    }
}
