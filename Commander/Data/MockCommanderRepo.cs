﻿using Commander.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commander.Data
{
    public class MockCommanderRepo : ICommanderRepo
    {
        public void CreateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }

        public void DeleteCommand(Command cmd)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommand()
        {
            var commands = new List<Command>
            {
                new Command
                {
                     Id = 0,
                     HowTo = "Boil an egg",
                     Line = "Boil Water",
                     Platform = "Kettle & Pan"
                },
                new Command
                {
                     Id = 1,
                     HowTo = "Cut bread",
                     Line ="Get a knife",
                     Platform = "Knife & chopping board"
                },
                new Command
                {
                     Id = 2,
                     HowTo = "Make a cup of tea",
                     Line = "Place tea bag in cup",
                     Platform = "Kettle & cup"
                }
            };

            return commands;
        }

        public Command GetCommandById(int id)
        {
            return new Command 
            {
                 Id = 0,
                 HowTo = "Boil an egg",
                 Line = "Boil Water",
                 Platform = "Kettle & Pan"
            };
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }
    }
}
