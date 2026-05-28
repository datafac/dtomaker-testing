using DTOMaker.Runtime.JsonSystemText;
using MasterMemory;
using Shouldly;
using Xperimental.CySharp.MemTables;

namespace Xperimental.CySharp.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Roundtrip_JST_MemTables()
        {
            MMPerson orig = new MMPerson
            {
                Id = new PersonId(1),
                Name = "Jack Flash",
                Age = 30,
                Gender = Gender.Male,
            };

            orig.Id.Native.ShouldBe(1);
            orig.Age.ShouldBe(30);
            orig.Name.ShouldBe("Jack Flash");
            orig.Gender.ShouldBe(Gender.Male);

            Xperimental.CySharp.JsonSystemText.Person send = new JsonSystemText.Person(orig);
            send.Freeze();

            string json = send.SerializeToJson();

            Xperimental.CySharp.JsonSystemText.Person? recd = json.DeserializeFromJson<Xperimental.CySharp.JsonSystemText.Person>();
            recd.ShouldNotBeNull();
            recd.Freeze();
            recd.ShouldBe(send);

            MMPerson copy = new MMPerson(recd);
            copy.ShouldBe(orig);

            copy.Id.Native.ShouldBe(1);
            copy.Age.ShouldBe(30);
            copy.Name.ShouldBe("Jack Flash");
            copy.Gender.ShouldBe(Gender.Male);
        }

        [Fact(Skip = "Fails because MMPerson is not a MessagePack object.")]
        public void MemTables_Db_Example()
        {
            // to create database, use DatabaseBuilder and Append method.
            var builder = new DatabaseBuilder();
            builder.Append(new MMPerson[]
            {
                new (){ Id = new PersonId(0), Age = 13, Gender = Gender.Male,   Name = "Dana Terry" },
                new (){ Id = new PersonId(1), Age = 17, Gender = Gender.Male,   Name = "Kirk Obrien" },
                new (){ Id = new PersonId(2), Age = 31, Gender = Gender.Male,   Name = "Wm Banks" },
                new (){ Id = new PersonId(3), Age = 44, Gender = Gender.Male,   Name = "Karl Benson" },
                new (){ Id = new PersonId(4), Age = 23, Gender = Gender.Male,   Name = "Jared Holland" },
                new (){ Id = new PersonId(5), Age = 27, Gender = Gender.Female, Name = "Jeanne Phelps" },
                new (){ Id = new PersonId(6), Age = 25, Gender = Gender.Female, Name = "Willie Rose" },
                new (){ Id = new PersonId(7), Age = 11, Gender = Gender.Female, Name = "Shari Gutierrez" },
                new (){ Id = new PersonId(8), Age = 63, Gender = Gender.Female, Name = "Lori Wilson" },
                new (){ Id = new PersonId(9), Age = 34, Gender = Gender.Female, Name = "Lena Ramsey" },
            });

            // build database binary(you can also use `WriteToStream` for save to file).
            byte[] data = builder.Build();

            // -----------------------

            // for query phase, create MemoryDatabase.
            // (MemoryDatabase is recommended to store in singleton container(static field/DI)).
            var db = new MemoryDatabase(data);

            // .PersonTable.FindByPersonId is fully typed by code-generation.
            MMPerson person = db.MMPersonTable.FindById(new PersonId(5));

            // Multiple key is also typed(***And * **), Return value is multiple if key is marked with `NonUnique`.
            RangeView<MMPerson> result = db.MMPersonTable.FindByGenderAndAge((Gender.Female, 23));

            // Get nearest value(choose lower(default) or higher).
            RangeView<MMPerson> age1 = db.MMPersonTable.FindClosestByAge(31);

            // Get range(min-max inclusive).
            RangeView<MMPerson> age2 = db.MMPersonTable.FindRangeByAge(20, 29);
        }
    }
}