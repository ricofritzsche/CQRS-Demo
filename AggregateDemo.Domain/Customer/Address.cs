namespace AggregateDemo.Domain.Customer
{
    using System;

    /// <summary>
    /// Das Adresse Value Object.
    /// </summary>
    [Serializable]
    public struct Address
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der Adresse Struktur.
        /// </summary>
        /// <param name="street">Die Strasse.</param>
        /// <param name="houseNumber">Die Hausnummer.</param>
        /// <param name="postalCode">Die Postleitzahl.</param>
        /// <param name="city">Der Name des Ortes.</param>
        public Address(string street, string houseNumber, string postalCode, string city)
            : this()
        {
            this.Street = street;
            this.HouseNumber = houseNumber;
            this.PostalCode = postalCode;
            this.City = city;
        }

        public string Street { get; private set; }

        public string HouseNumber { get; private set; }

        public string PostalCode { get; private set; }

        public string City { get; private set; }
    }
}