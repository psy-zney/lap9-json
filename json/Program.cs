using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace json
{
    

    [Serializable]
    public class Student : ISerializable
    {
        public int id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public Student() { }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", id);
            info.AddValue("name", name);
            info.AddValue("age", age);
        }
        public Student(SerializationInfo info, StreamingContext context)
        {
            id = info.GetInt32("id");
            name = info.GetString("name");
            age = info.GetInt32("age");
        }
        public override string ToString()
        {
            return $"Student : Id {id}, Name: {name}, Age : {age}";
        }
    }

    [Serializable]
    public class StudentList : ISerializable
    {
        private List<Student> students = new List<Student>();
        public List<Student> Students { get => students; set { students = value; } }
        public StudentList() { }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Student", Students);
        }
        public StudentList(SerializationInfo info, StreamingContext context)
        {
            students = (List<Student>)info.GetValue("Students", typeof(List<Student>));
        }
        public void Add(Student item)
        {
            Students.Add(item);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            StudentList studentlist = new StudentList();
            studentlist.Add(new Student { id = 1, name = "khanh", age = 17 });
            studentlist.Add(new Student { id = 2, name = "Linh", age = 17 });
            studentlist.Add(new Student { id = 3, name = "anh", age = 17 });
            studentlist.Add(new Student { id = 4, name = "van", age = 17 });

            //Chuyển đổi danh sách sinh viên (StudentList) thành định dạng JSON và ghi vào file.
            string json = JsonSerializer.Serialize(studentlist, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("data.dat", json);

            // Đọc dữ liệu JSON từ file.
            string newjson = File.ReadAllText("data.dat");

            // Chuyển đổi dữ liệu JSON đọc được thành một danh sách sinh viên mới.
            StudentList stlist = JsonSerializer.Deserialize<StudentList>(newjson);

            // Thêm các sinh viên mới vào danh sách sinh viên.
            stlist.Add(new Student { id = 8, name = "hoa", age = 18 });
            stlist.Add(new Student { id = 6, name = "hong", age = 18 });

            // Chuyển đổi danh sách sinh viên đã cập nhật thành JSON và ghi đè lên file cũ.
            json = JsonSerializer.Serialize(stlist, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("data.dat", json);

            //Đọc dữ liệu JSON đã cập nhật từ file.
            newjson = File.ReadAllText("data.dat");

            // Chuyển đổi dữ liệu JSON đã cập nhật thành một danh sách sinh viên mới và hiển thị kết quả ra màn hình.
            stlist = JsonSerializer.Deserialize<StudentList>(newjson);
            Console.WriteLine("\nUpdated Students:");
            foreach (Student student in stlist.Students)
            {
                Console.WriteLine(student);
            }
           
            Console.ReadLine();
        }
    }
}

