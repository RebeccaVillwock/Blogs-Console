using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                string loopFlag = "1";
                string postSearch;
                string userInput;
                var db = new BloggingContext();
                //loop through choices until user is done
                while (loopFlag == "1" || loopFlag == "2" || loopFlag == "3" || loopFlag == "4")
                {
                    //choices
                    Console.WriteLine("1) Display all blogs");
                    Console.WriteLine("2) Add Blog");
                    Console.WriteLine("3) Create Post");
                    Console.WriteLine("4) Display Posts");

                    loopFlag = Console.ReadLine();

                    //int for lableing purposes
                    int num = 1;

                    //"Display all blogs"
                    if (loopFlag == "1") {
                        // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            
                            Console.WriteLine(item.Name);
                        }
                    }
                    //"Add blog
                    else if(loopFlag == "2")
                    {
                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();

                        var blog = new Blog { Name = name };


                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                    }
                    //"Create Post"
                    else if(loopFlag == "3")
                    {
                        
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(num + ")" + item.Name);
                            num++;
                        }
                        //return lable to 0 so it can be used later
                        num = 0;
                        //desired search method
                        Console.WriteLine("Enter Desired Blog Name");
                        
                        userInput = Console.ReadLine();
                        userInput = userInput.ToLower();

                        //check if user selection exists
                        //if not or if they didnt enter anything ask again
                        var blogExists = db.Blogs.Any(b => b.Name.ToLower() == userInput);

                        while (userInput == null || !blogExists) {
                            Console.WriteLine("Sorry that blog does not exist please try again");
                            Console.WriteLine("Desired Blog Name");
                            userInput = Console.ReadLine();
                            userInput = userInput.ToLower();

                            //check if user selection exists
                            //if not or if they didnt enter anything ask again
                            blogExists = db.Blogs.Any(b => b.Name.ToLower() == userInput);
                            
                        }

                        //TODO verification
                        //connect to the desired blog
                        var blog = db.Blogs.FirstOrDefault(b => b.Name.ToLower() == userInput);
                        
                            
                            

                        //create the post to hold the info
                        var post = new Post();

                        //collect the desired info
                        Console.WriteLine("Title: ");
                        userInput = Console.ReadLine();

                        post.Title = userInput;

                        Console.WriteLine("Content: ");
                        userInput = Console.ReadLine();

                        post.Content = userInput;
                        post.BlogId = blog.BlogId;

                        //add post to blog
                        db.AddPost(post);

                    }
                    //DisplayPosts
                    else if(loopFlag == "4"){

                        Console.WriteLine("Enter name of blog or 'ALL' to see all posts");
                        //ask what posts they would like to see
                        Console.WriteLine("0) Posts from all blogs");

                        //display blog options for posts
                        var query = db.Blogs.OrderBy(b => b.Name);
                        foreach (var item in query)
                        {
                            Console.WriteLine(num + ") Posts from " + item.Name);
                            num++;
                        }
                        num = 0;

                        //blog(s) user wants posts from
                        userInput = Console.ReadLine().ToLower();

                        

                        //check if user selection exists
                        //if not or if they didnt enter anything ask again
                        var blogExists = db.Blogs.Any(b => b.Name.ToLower() == userInput);

                        while (!blogExists && !(userInput == "all"))
                        {
                            Console.WriteLine("Sorry that blog does not exist please try again");
                            Console.WriteLine("Desired Blog Name");
                            userInput = Console.ReadLine();
                            userInput = userInput.ToLower();

                            //check if user selection exists
                            //if not or if they didnt enter anything ask again
                            blogExists = db.Blogs.Any(b => b.Name.ToLower() == userInput);

                        }

                        //if user wants all posts
                        if (userInput == "all")
                        {
                            
                            var postQuery = db.Posts.OrderBy(p => p.BlogId);
                            foreach(var item in postQuery)
                            {
                                Console.WriteLine(num + ") " + item.Title);
                                Console.WriteLine(item.Content);
                                num++;
                            }

                           
                        }
                        //if user wants posts from specific blog
                        else
                        {
                            var chosenBlog = db.Blogs.Where(b => b.Name == userInput).ToList();
                            int blogID = 0;
                            //get blog id to find appropriate posts
                            foreach(var blog in chosenBlog)
                            {
                                blogID = blog.BlogId;
                            }

                            var desiredPosts = db.Posts.Where(p => p.BlogId == blogID);
                            //display posts
                            foreach(var item in desiredPosts)
                            {
                                Console.WriteLine(num + ") " + item.Title);
                                Console.WriteLine(item.Content);
                                num++;
                            }
                            num = 0;
                        }





                        //still need to post what was selected



                        //var query = db.Blogs.OrderBy(b => b.Name);

                        //Console.WriteLine("All blogs in the database:");
                        //foreach (var item in query)
                        //{

                          //  Console.WriteLine(item.Name);
                        //}











                    }


                    


                }

                

               

                
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}
