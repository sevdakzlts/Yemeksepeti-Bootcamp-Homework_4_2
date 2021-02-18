using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homework_4_2.API.Models;

namespace Homework_4_2.API.Data
{
    public class DummyData
    {
        private static volatile DummyData _instance = null;
        private static readonly object padLock = new object();

        public static DummyData Instance
        {
            get
            {
                lock (padLock)
                {
                    if (_instance == null)
                    {
                        _instance = new DummyData();
                    }
                }
                return _instance;
            }
        }

        private DummyData()
        {
            FillData();
        }

        private void FillData()
        {
            for (int i = 1; i <= 10; i++)
            {
                Users.Add(new UserModel() { Id = i, Name = "User_" + i });
            }
        }

        public List<UserModel> Users = new List<UserModel>();
    }
}

