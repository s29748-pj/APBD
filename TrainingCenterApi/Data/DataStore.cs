using TrainingCenterApi.Models;

namespace TrainingCenterApi.Data;

public static class DataStore
{
    public static List<Room> Rooms = new()
    {
        new Room
        {
            Id = 1,
            Name = "Lab 101",
            BuildingCode = "A",
            Floor = 1,
            Capacity = 20,
            HasProjector = true,
            IsActive = true
        },

        new Room
        {
            Id = 2,
            Name = "Lab 204",
            BuildingCode = "B",
            Floor = 2,
            Capacity = 30,
            HasProjector = true,
            IsActive = true
        },

        new Room
        {
            Id = 3,
            Name = "Meeting Room",
            BuildingCode = "A",
            Floor = 3,
            Capacity = 12,
            HasProjector = false,
            IsActive = true
        },

        new Room
        {
            Id = 4,
            Name = "Workshop X",
            BuildingCode = "C",
            Floor = 1,
            Capacity = 40,
            HasProjector = true,
            IsActive = false
        }
    };

    public static List<Reservation> Reservations = new();
}