using Famicom.Components.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModify
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var tableRepository = new TableRepository();
            var scheduleRepository = new ScheduleRepository();
            var presetRepository = new PresetRepository();
            var apiRepository = new ApiRepository();
            var user = new UserRepository();
            var roomRepository = new RoomRepository();

            // Insert
            string jsonbData = "{\"key1\":\"value1\", \"key2\":\"value2\"}";
            string jsonData = "{\"key1\":\"value1\", \"key2\":\"value2\"}";
            apiRepository.InsertApi("Test", jsonbData);
            apiRepository.InsertApi("Test2", jsonData);
            string permisions = "{\"read\":\"true\", \"save\":\"false\"}";
            user.InsertUserType("Commoner", permisions);
            user.InsertUser("Test", "Test@gmail.pl", 1);
            user.InsertUser("Test2", "King", 1);
            roomRepository.InsertRoom("Kitchen", "23", 2);
            tableRepository.InsertTable("Table1", "Manufacturer1", 1);
            




            //List<Rooms> rooms = roomRepository.GetRooms();

            //foreach (var room in rooms)
            // {
             //    Console.WriteLine(room.RoomName);
            //     Console.WriteLine(room.RoomNumber);
            // }

            //List<User> users = user.GetUser("King");

           // foreach (var usert in users)
           // {
           //     Console.WriteLine(usert.UserName);
           //     Console.WriteLine(usert.UserEmail);
                
           // }



            List<Tables> tables = tableRepository.GetTablesRoom(1);
            foreach (var table in tables)
            {
                Console.WriteLine(table.TableName);
                Console.WriteLine(table.TableManufacturer);
            }





            // Delete
            //tableRepository.DeleteTable(1);
            //scheduleRepository.DeleteSchedule(1);
            //presetRepository.DeletePreset(1);
            //apiRepository.DeleteApi(1);
        }
    }
}
