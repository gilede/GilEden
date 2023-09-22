
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;

namespace BackendTesting
{
    internal class UserTests
    {
        private readonly UserService user;
        public readonly TaskService taskService;
        public readonly BoardService boardService;


        public UserTests(UserService u, BoardService b, TaskService t)
        {
            this.user = u;
            this.taskService = t;
            this.boardService = b;
        }

        public void RunTestsRegister()
        {

            Console.WriteLine("~Register Test~");
            //register amit@gmail.com to the Kanban - should succeed
            //assuming that there is no user with this email in the kanban
            string res1 = user.Register("amit@gmail.com", "ABCabc123");
            Response response1 = JsonSerializer.Deserialize<Response>(res1);
            if (response1.ErrorOccured)
            {
                Console.WriteLine(response1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created new user successfully");
            }



            //attempt to register amit@gmail.com - should return Error
            //assuming that user with this email is already exist in the kanban
            string res2 = user.Register("amit@gmail.com", "ABCabc123");
            var response2 = JsonSerializer.Deserialize<Response>(res2);
            if (response2.ErrorOccured)
            {
                Console.WriteLine(response2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created new user successfully");
            }

            //attempt to register with an empty Email field - should return Error
            string res3 = user.Register(null, "1919");
            var response3 = JsonSerializer.Deserialize<Response>(res3);

            if (response3.ErrorOccured)
            {
                Console.WriteLine(response3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created new user successfully");
            }



            //register gil@gmail.com to the Kanban - should succeed
            //assuming that there is no user with this email in the kanban
            string res4 = user.Register("gil@gmail.com", "ABCabc123");
            Response response4 = JsonSerializer.Deserialize<Response>(res4);
            if (response4.ErrorOccured)
            {
                Console.WriteLine(response4.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created new user successfully");
            }

            string res_reg = user.Register("gileden@gmail.com", "ABCabc123");
            Response response6 = JsonSerializer.Deserialize<Response>(res_reg);
            if (response6.ErrorOccured)
            {
                Console.WriteLine(response6.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created new user successfully");
            }


            Console.WriteLine("");



        }

        public void RunTestsLogin()
        {

            Console.WriteLine("~Login Test~");
            //attempt to login 'amit@gmail.com' user with the incorrect password - should return Error
            //assuming that user with this email exist in the kanban with other password
            string res5 = user.Login("amit@gmail.com", "1235");
            var response5 = JsonSerializer.Deserialize<Response>(res5);

            if (response5.ErrorOccured)
            {
                Console.WriteLine(response5.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logged user in successfully");
            }

            //attempt to login amit@gmail.com user with the correct password - should succced
            //assuming that user with this email and password exist in the kanban and logged out
            string res6 = user.Login("amit@gmail.com", "ABCabc123");
            var response6 = JsonSerializer.Deserialize<Response>(res6);

            if (response6.ErrorOccured)
            {
                Console.WriteLine(response6.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logged user in successfully");
            }


            //attempt to login amit@bgu.com - should return Error
            //assuming that 'amit@bgu.com' user does not exist
            string res7 = user.Login("amit@bgu.com", "1235");
            var response7 = JsonSerializer.Deserialize<Response>(res7);

            if (response7.ErrorOccured)
            {
                Console.WriteLine(response7.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logged user in successfully");
            }


            //attempt to login with empty username field - should return Error
            string res8 = user.Login(null, "1235");
            var response8 = JsonSerializer.Deserialize<Response>(res8);

            if (response8.ErrorOccured)
            {
                Console.WriteLine(response8.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logged user in successfully");
            }

            //attempt to login gil@gmail.com user with the correct password - should succced
            //assuming that user with this email and password exist in the kanban and logged out
            string res9 = user.Login("gil@gmail.com", "ABCabc123");
            var response9 = JsonSerializer.Deserialize<Response>(res9);


            if (response9.ErrorOccured)
            {
                Console.WriteLine(response9.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logged user in successfully");
            }
            Console.WriteLine("");

        }

        public void RunTestPassword()    //attempt to change password for amit@gmail.com user with the wrong old password - should fails 

        {

            Console.WriteLine("~Password Test~");
            string res13 = user.ChangePassword("amit@gmail.com", "123", "Aa12fkfea");
            var response13 = JsonSerializer.Deserialize<Response>(res13);

            if (response13.ErrorOccured)
            {
                Console.WriteLine(response13.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Changed user password successfully");
            }
            //attempt to change password for amit@gmail.com user with the correct old password - should work 
            string res14 = user.ChangePassword("amit@gmail.com", "ABCabc123", "Aa12fkfea");
            var response14 = JsonSerializer.Deserialize<Response>(res14);

            if (response14.ErrorOccured)
            {
                Console.WriteLine(response14.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Changed user password successfully");
            }
            Console.WriteLine("");

        }

        public void RunTestsLogout()
        {

            Console.WriteLine("~Logout Test~");
            //attempt to logout with 'gil@gmail.com' when the user is logged in-should succesed
            string res10 = user.Logout("amit@gmail.com");
            var response10 = JsonSerializer.Deserialize<Response>(res10);

            if (response10.ErrorOccured)
            {
                Console.WriteLine(response10.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Loggedout user successfully");
            }

            //try to logout with 'amit@gmail.com' when the user is logged out - should failed
            string res11 = user.Logout("amit@gmail.com");
            var response11 = JsonSerializer.Deserialize<Response>(res11);

            if (response11.ErrorOccured)
            {
                Console.WriteLine(response11.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logged out user successfully");
            }

            //attempt to logout with 'gil@gmail.com' when the user is logged in-should succesed
            string res12 = user.Logout("gil@gmail.com");
            var response12 = JsonSerializer.Deserialize<Response>(res12);

            if (response12.ErrorOccured)
            {
                Console.WriteLine(response12.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Logged out user successfully");
            }
            Console.WriteLine("");

        }


        public void testGetAlluserTask()
        {
            Console.WriteLine("~getAllUserTask Test~");
            boardService.CreateBoard("amit@gmail.com", "amit");
            //taskService.AddTask("amit@gmail.com", "amit", new DateTime(2025, 4, 01), "task1", "do the dishes");
            boardService.AdvanceTask("amit@gmail.com", "amit", 0, 0);

            string res15 = user.gellallCurrentCol("giles@gmail.com");
            var response15 = JsonSerializer.Deserialize<Response>(res15);
            if (response15.ErrorOccured)
            {
                Console.WriteLine(response15.ErrorMessage);
            }
            else
            {
                Console.WriteLine($"all current tests :  {response15.ReturnValue}");
            }

            Console.WriteLine("");
        }


        public void testGetUserBoards()
        {

            Console.WriteLine("~GetUserBoards Test~");
            string res16 = user.GetUserBoards("giles@gmail.com");   // should be successfull because giles@gmail,com has existing boards 
            var response16 = JsonSerializer.Deserialize<Response>(res16);
            if (response16.ErrorOccured)
            {
                Console.WriteLine(response16.ErrorMessage);
            }
            else
            {
                Console.WriteLine($"all current boards :  {response16.ReturnValue}");
            }

            Console.WriteLine("");


            string res16_ = user.GetUserBoards("noakirel@gmail.com");   // should be unsuccessfull because noakirel@gmail doesn't exist in the system 
            var response16_ = JsonSerializer.Deserialize<Response>(res16_);
            if (response16_.ErrorOccured)
            {
                Console.WriteLine(response16.ErrorMessage);
            }
            else
            {
                Console.WriteLine($"all current boards :  {response16_.ReturnValue}");
            }

            Console.WriteLine("");
        }

        public void testJoinBoard()
        {

            Console.WriteLine("~JoinBoard Test~");
            string res17 = boardService.JoinBoard("giles@gmail.com",12);   //should be successfull beause boardId 12 exist . 
            var response17 = JsonSerializer.Deserialize<Response>(res17);
            if (response17.ErrorOccured)
            {
                Console.WriteLine(response17.ErrorMessage);
            }
            else
            {
                Console.WriteLine($"Join Board Successfully :  {response17.ReturnValue}");
            }

            Console.WriteLine("");


            string res17_ = boardService.JoinBoard("giles@gmail.com", 2345);   //should be unsuccessfull beause boardId 2345 doesn't exist . 
            var response17_ = JsonSerializer.Deserialize<Response>(res17_);
            if (response17_.ErrorOccured)
            {
                Console.WriteLine(response17_.ErrorMessage);
            }
            else
            {
                Console.WriteLine($"Join Board Successfully :  {response17_.ReturnValue}");
            }

            Console.WriteLine("");
        }
        public void testLeaveBoard()
        {

            Console.WriteLine("~LeaveBoard Test~");
            string res18 = boardService.LeaveBoard("giles@gmail.com",12);  //should be successfull beause boardId 12 exist .  
            var response18 = JsonSerializer.Deserialize<Response>(res18);
            if (response18.ErrorOccured)
            {
                Console.WriteLine(response18.ErrorMessage);
            }
            else
            {
                Console.WriteLine($"all current tests :  {response18.ReturnValue}");
            }

            Console.WriteLine("");

            string res18_ = boardService.LeaveBoard("giles@gmail.com", 134);  //should be unsuccessfull beause 134 doesnt exist/ user hasn't joined this board .  
            var response18_ = JsonSerializer.Deserialize<Response>(res18_);
            if (response18_.ErrorOccured)
            {
                Console.WriteLine(response18_.ErrorMessage);
            }
            else
            {
                Console.WriteLine($"all current tests :  {response18_.ReturnValue}");
            }

            Console.WriteLine("");


        }
        public void testassignTask()
        {
            Console.WriteLine("~AssignTask Test~");
            string res19 = boardService.AssignTask("giles@gmail.com","Board12",2,23, "gileden@gmail.com"); //should assign task successfully from origin  
            var response19 = JsonSerializer.Deserialize<Response>(res19); //user to the assigned user . 
            if (response19.ErrorOccured)
            {
                Console.WriteLine(response19.ErrorMessage);
            }
            else
            {
                Console.WriteLine($"assigned the task :  {response19.ReturnValue}");
            }

            Console.WriteLine("");


            string res19_ = boardService.AssignTask("giles@gmail.com", "Board12", 2, 23, "noakirel@gmail.com"); //shouldn't  assign task successfully the user 
            var response19_ = JsonSerializer.Deserialize<Response>(res19_); // because noakirel@gmail.com doesn't exist . 
            if (response19_.ErrorOccured)
            {
                Console.WriteLine(response19_.ErrorMessage);
            }
            else
            {
                Console.WriteLine($"assigned the task :  {response19.ReturnValue}");
            }

            Console.WriteLine("");
        }
        public void testLoadData()        // there can't be rainy test for loadData for now (no DB assigned). 
        {

            Console.WriteLine("~LoadData Test~");
            string res20 = boardService.LoadData();
            var response20 = JsonSerializer.Deserialize<Response>(res20);
            if (response20.ErrorOccured)
            {
                Console.WriteLine(response20.ErrorMessage);
            }
            else
            {
                Console.WriteLine($"Loaded the data :  {response20.ReturnValue}");
            }

            Console.WriteLine("");

         
        }

        public void testDeleteData()  //  there can't be rainy test for DeleteData for now (no DB assigned).
        {
            Console.WriteLine("~DeleteData Test~");
            string res21 = boardService.DeleteData();
            var response21 = JsonSerializer.Deserialize<Response>(res21);
            if (response21.ErrorOccured)
            {
                Console.WriteLine(response21.ErrorMessage);
            }
            else
            {
                Console.WriteLine($"Deleted all Data :  {response21.ReturnValue}");
            }

            Console.WriteLine("");
        }
    }
}
