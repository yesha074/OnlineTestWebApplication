using project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace project.Controllers
{
    public class HomeController : Controller
    {
        onlinetestEntities db = new onlinetestEntities();
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult sregister()
        {
            return View();
        }
        [HttpPost]
        public ActionResult sregister(STUDENT stu,HttpPostedFileBase imgfile)
        {
            STUDENT s = new STUDENT();
            try
            {
                s.STUDENT_NAME = stu.STUDENT_NAME;
                s.STUDENT_PASSWORD = stu.STUDENT_PASSWORD;
                s.STUDENT_IMAGE = uploadimage(imgfile);
                db.STUDENT.Add(s);
                db.SaveChanges();
                return RedirectToAction("slogin");
            }
            catch(Exception)
            {
                ViewBag.msg = "Sorry!, Data could not be Inserted...";
            }
            return View();
        }
        public string uploadimage(HttpPostedFileBase file)
        {
            Random r = new Random();
            String path= "-1";
            int random = r.Next();

                if(file != null && file.ContentLength>0)
                {
       
                    string extension = Path.GetExtension(file.FileName);
                    if(extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                    {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/images"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/images/" + random + Path.GetFileName(file.FileName);
                    }
                    catch(Exception ex)
                    {
                        path = "-1";
                    }
                    }
                else
                {
                    Response.Write("<script>alert('Only JPG, JPEG, PNG formates are Acceptable...')</script>");
                }
                }
           
            return path;
        }
        [HttpGet]
        public ActionResult tlogin()
        {
             return View();
        }
       [HttpPost]
        public ActionResult tlogin(ADMIN a)
        {
            ADMIN ad = db.ADMIN.Where(x => x.ADMIN_NAME == a.ADMIN_NAME && x.ADMIN_PASSWORD == a.ADMIN_PASSWORD).SingleOrDefault();
            if(ad!=null)
            {
                Session["ADMIN_ID"] = ad.ADMIN_ID;
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.msg = "Invalid USERNAME or PASSWORD! , Please Try Again!...";
            }
            return View();
        }
        [HttpGet]
        public ActionResult slogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult slogin(STUDENT s)
        {
            STUDENT std = db.STUDENT.Where(x => x.STUDENT_NAME == s.STUDENT_NAME && x.STUDENT_PASSWORD == s.STUDENT_PASSWORD).SingleOrDefault();
            if(std==null)
            {
                ViewBag.msg = "Invalid Email or PassWord!";
            }
            else
            {
                Session["std_id"] = std.STUDENT_ID;
                return RedirectToAction("Instructions");
            }
            return View();
        }
        public ActionResult Instructions()
        {
            if (Session["std_id"] == null)
            {
                return RedirectToAction("slogin");
            }
            return View(db.CATEGORY);
        }
       public ActionResult ViewAllQuestions(int? id)
        {
            if (Session["ADMIN_ID"] == null)
            {
                
                return RedirectToAction("tlogin");
            }
            if (id==null)
            {
                return RedirectToAction("Dashboard");
            }
     
            return View(db.QUESTIONS.Where(x => x.QUE_FKEY_CATEGORYID == id).ToList());
        }
            public ActionResult StudentExam()
        {
            if (Session["std_id"] == null)
            {
                return RedirectToAction("slogin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult StudentExam(string room)
        {
           List<CATEGORY> list = db.CATEGORY.ToList();
            TempData["score"] = 0;
            foreach(var item in list)
            {
                if (item.CATEGORY_ENC_STRING==room)
                {
                    List<QUESTION> li = db.QUESTIONS.Where(x => x.QUE_FKEY_CATEGORYID == item.CATEGORY_ID).ToList();
                    Queue<QUESTION> queue = new Queue<QUESTION>();
                    foreach(QUESTION a in li)
                        {
                        queue.Enqueue(a);
                        }
                    TempData["exam_id"] = item.CATEGORY_ID;
                    TempData["questions"] = queue;
                    TempData["score"] = 0;
                    TempData.Keep();
                    return RedirectToAction("TestStart");
                }
                else
                {
                    ViewBag.error = "No Category Found";
                }
            }
           
            return View();
        }
        public ActionResult TestStart()
        {
            if (Session["std_id"] == null)
            {
                return RedirectToAction("slogin");
            }
            if (TempData["questions"] == null)
            {
                return RedirectToAction("StudentExam");
            }


            TempData.Keep();

            QUESTION q = null;
            if(TempData["questions"]!=null)
            { 
                Queue<QUESTION> qlist = (Queue<QUESTION>)TempData["questions"];

                if(qlist.Count>0)
                {
                    q = qlist.Peek();
                    qlist.Dequeue();
                    TempData["questions"] = qlist;
                    return View(q);
                }
                else
                {
                    return RedirectToAction("EndExam");
                }
            }
            return View();
        }
       [HttpPost]
        public ActionResult TestStart(QUESTION q)
        {
            string correct_ans=null;
            if (q.OPTIONA == null && q.OPTIONB == null && q.OPTIONC == null && q.OPTIOND == null)
            {
                Response.Write("<script>alert('Please Select One Option...')</script>");

            }
            else
            { 
            if (q.OPTIONA!=null)
            {
                correct_ans = "A"; 
            }
            else if(q.OPTIONB!=null)
            {
                correct_ans = "B";
            }
            else if (q.OPTIONC != null)
            {
                correct_ans = "C";
            }
            else if (q.OPTIOND != null)
            {
                correct_ans = "D";
            }
            if(correct_ans.Equals(q.COREECT_OPTION))
            {
                TempData["score"] = Convert.ToInt32(TempData["score"]) + 1;
            }
            }
            TempData.Keep();
            return RedirectToAction("TestStart");
          
        }
        [HttpGet]
        public ActionResult StudentLogout()
        {
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("slogin");
        }
        public ActionResult EndExam()
        {
            if (Session["std_id"] == null)
            {
                return RedirectToAction("slogin");
            }

            return View();
        }
        public ActionResult Dashboard()
        {
            if (Session["ADMIN_ID"] == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Addcategory()
        {
            if (Session["ADMIN_ID"] == null)
            {
                return RedirectToAction("Index");
            }
            //Session["ADMIN_ID"] = 1;
            int ADMIN_ID = Convert.ToInt32(Session["ADMIN_ID"].ToString());
            List<CATEGORY> li = db.CATEGORY.Where(x =>x.CATEGORY_FKEY_ADMINID== ADMIN_ID).OrderByDescending(x => x.CATEGORY_ID).ToList();
            ViewData["list"] = li;
            return View();
        }
        [HttpPost]
        public ActionResult Addcategory(CATEGORY cat)
        {
            
            List<CATEGORY> li = db.CATEGORY.OrderByDescending(x => x.CATEGORY_ID).ToList();
            ViewData["list"] = li;
            Random r = new Random();
            CATEGORY c = new CATEGORY();
            c.CATEGORY_NAME = cat.CATEGORY_NAME;
            c.CATEGORY_ENC_STRING = crypto.Encrypt(cat.CATEGORY_NAME.Trim() + r.Next().ToString(), true);
            c.CATEGORY_FKEY_ADMINID = Convert.ToInt32(Session["ADMIN_ID"].ToString());
            db.CATEGORY.Add(c);
            db.SaveChanges();

            return RedirectToAction("Addcategory");
        }
        [HttpGet]
        public ActionResult Addquestions()
        {
            if (Session["ADMIN_ID"] == null)
            {
                return RedirectToAction("Index");
            }
            int sess_id = Convert.ToInt32(Session["ADMIN_ID"]);
            List<CATEGORY> li = db.CATEGORY.Where(x => x.CATEGORY_FKEY_ADMINID == sess_id).ToList();
            ViewBag.list = new SelectList(li, "CATEGORY_ID", "CATEGORY_NAME");
            return View();
        }
        [HttpPost]
        public ActionResult Addquestions(QUESTION que)
        {
            int sess_id = Convert.ToInt32(Session["ADMIN_ID"]);
            List<CATEGORY> li = db.CATEGORY.Where(x => x.CATEGORY_FKEY_ADMINID == sess_id).ToList();
            ViewBag.list = new SelectList(li, "CATEGORY_ID", "CATEGORY_NAME");
            QUESTION q = new QUESTION();
            q.QUESTION_TEXT = que.QUESTION_TEXT;
            q.OPTIONA = que.OPTIONA;
            q.OPTIONB = que.OPTIONB;
            q.OPTIONC = que.OPTIONC;
            q.OPTIOND = que.OPTIOND;
            q.COREECT_OPTION = que.COREECT_OPTION;
            q.QUE_FKEY_CATEGORYID = que.QUE_FKEY_CATEGORYID;
            db.QUESTIONS.Add(q);
            db.SaveChanges();
            TempData["msg"] = "Question Added Successfully!...";
            TempData.Keep();
            return RedirectToAction("Addquestions");
           
        }
       [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Session["ADMIN_ID"] == null)
            {
                return RedirectToAction("Index");
            }
            var que = db.QUESTIONS.Where(x => x.QUESTION_ID == id).FirstOrDefault();

            return View(que);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include =
        "QUESTION_ID,QUESTION_TEXT,OPTIONA,OPTIONB,OPTIONC,OPTIOND,COREECT_OPTION,QUE_FKEY_CATEGORYID")] QUESTION Q)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Q).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Addcategory");
            }
            return View(Q);
        }
        public ActionResult Delete(int? id)
        {
            if (Session["ADMIN_ID"] == null)
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return HttpNotFound();
            }
            QUESTION q = db.QUESTIONS.Find(id);
            if (q == null)
            {
                return HttpNotFound();
            }
            return View(q);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QUESTION q = db.QUESTIONS.Find(id);
            db.QUESTIONS.Remove(q);
            db.SaveChanges();
            return RedirectToAction("Addcategory");
        }
        public ActionResult Index()
        {
            if(Session["ADMIN_ID"]!=null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}