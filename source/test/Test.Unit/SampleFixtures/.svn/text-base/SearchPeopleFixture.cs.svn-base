using System.Runtime.Serialization;

namespace Test.Unit.SampleFixtures
{
    public class SearchPeopleFixture
    {
        public Person[] Search(SearchPeopleParameters parameters)
        {
            return PretendToRetrievePeople(parameters);
        }

        public static Person[] PretendToRetrievePeople(SearchPeopleParameters parameters)
        {
            if (parameters.FirstName.Equals("Wolfgang"))
                return new Person[] {};

            Person person1 = new Person(parameters.FirstName, "Albert", parameters.LastName);
            Person person2 = new Person(parameters.FirstName, "Barney", parameters.LastName);
            return new[] { person1, person2 };
        }
    }

    public class SearchPeopleNoActionParameter
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public Person[] Search()
		{
			return Search(new SearchPeopleParameters { FirstName = FirstName, LastName = LastName });
		}

		private static Person[] Search(SearchPeopleParameters searchPeopleParameters)
		{
			return SearchPeopleFixture.PretendToRetrievePeople(searchPeopleParameters);
		}
	}

    [DataContract]
	public class SearchPeopleParameters
	{
		[DataMember]
		public string FirstName { get; set; }
		[DataMember]
		public string LastName { get; set; }
	}

	public class Person
	{
		public string FirstName { get; private set; }
		public string MiddleName { get; private set; }
		public string LastName { get; private set; }

		public Person(string firstName, string middleName, string lastName)
		{
			FirstName = firstName;
			MiddleName = middleName;
			LastName = lastName;
		}
	}
}