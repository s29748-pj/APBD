using Microsoft.AspNetCore.Mvc;
using TrainingCenterApi.Data;
using TrainingCenterApi.Models;

namespace TrainingCenterApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(DataStore.Rooms);
    }
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var room = DataStore.Rooms
            .FirstOrDefault(x => x.Id == id);

        if (room == null)
            return NotFound();

        return Ok(room);
    }
    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuilding(string buildingCode)
    {
        var rooms = DataStore.Rooms
            .Where(x => x.BuildingCode == buildingCode)
            .ToList();

        return Ok(rooms);
    }
    [HttpGet("filter")]
    public IActionResult Filter(
        int? minCapacity,
        bool? hasProjector,
        bool? activeOnly)
    {
        var query = DataStore.Rooms.AsQueryable();

        if (minCapacity.HasValue)
        {
            query = query.Where(x =>
                x.Capacity >= minCapacity.Value);
        }

        if (hasProjector.HasValue)
        {
            query = query.Where(x =>
                x.HasProjector == hasProjector.Value);
        }

        if (activeOnly == true)
        {
            query = query.Where(x =>
                x.IsActive);
        }

        return Ok(query.ToList());
    }
    [HttpPost]
    public IActionResult Create(Room room)
    {
        room.Id = DataStore.Rooms.Max(x => x.Id) + 1;

        DataStore.Rooms.Add(room);

        return CreatedAtAction(
            nameof(GetById),
            new { id = room.Id },
            room);
    }
    [HttpPut("{id}")]
    public IActionResult Update(int id, Room updatedRoom)
    {
        var room = DataStore.Rooms
            .FirstOrDefault(x => x.Id == id);

        if (room == null)
            return NotFound();

        room.Name = updatedRoom.Name;
        room.BuildingCode = updatedRoom.BuildingCode;
        room.Floor = updatedRoom.Floor;
        room.Capacity = updatedRoom.Capacity;
        room.HasProjector = updatedRoom.HasProjector;
        room.IsActive = updatedRoom.IsActive;

        return Ok(room);
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var room = DataStore.Rooms
            .FirstOrDefault(x => x.Id == id);

        if (room == null)
            return NotFound();

        bool hasReservations =
            DataStore.Reservations.Any(x =>
                x.RoomId == id);

        if (hasReservations)
            return Conflict();

        DataStore.Rooms.Remove(room);

        return NoContent();
    }
}