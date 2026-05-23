using Microsoft.AspNetCore.Mvc;
using TrainingCenterApi.Data;
using TrainingCenterApi.Models;

namespace TrainingCenterApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll(
        DateOnly? date,
        string? status,
        int? roomId)
    {
        var query = DataStore.Reservations.AsQueryable();

        if (date.HasValue)
        {
            query = query.Where(x =>
                x.Date == date.Value);
        }

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(x =>
                x.Status == status);
        }

        if (roomId.HasValue)
        {
            query = query.Where(x =>
                x.RoomId == roomId.Value);
        }

        return Ok(query.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var reservation = DataStore.Reservations
            .FirstOrDefault(x => x.Id == id);

        if (reservation == null)
            return NotFound();

        return Ok(reservation);
    }

    [HttpPost]
    public IActionResult Create(Reservation reservation)
    {
        var room = DataStore.Rooms
            .FirstOrDefault(x => x.Id == reservation.RoomId);

        if (room == null)
            return NotFound("Room not found");

        if (!room.IsActive)
            return Conflict("Room is inactive");

        bool overlap =
            DataStore.Reservations.Any(x =>
                x.RoomId == reservation.RoomId &&
                x.Date == reservation.Date &&
                reservation.StartTime < x.EndTime &&
                reservation.EndTime > x.StartTime);

        if (overlap)
            return Conflict("Time conflict");

        reservation.Id =
            DataStore.Reservations.Any()
            ? DataStore.Reservations.Max(x => x.Id) + 1
            : 1;

        DataStore.Reservations.Add(reservation);

        return CreatedAtAction(
            nameof(GetById),
            new { id = reservation.Id },
            reservation);
    }

    [HttpPut("{id}")]
    public IActionResult Update(
        int id,
        Reservation updatedReservation)
    {
        var reservation = DataStore.Reservations
            .FirstOrDefault(x => x.Id == id);

        if (reservation == null)
            return NotFound();

        var room = DataStore.Rooms
            .FirstOrDefault(x => x.Id == updatedReservation.RoomId);

        if (room == null)
            return NotFound("Room not found");

        if (!room.IsActive)
            return Conflict("Room is inactive");

        bool overlap =
            DataStore.Reservations.Any(x =>
                x.Id != id &&
                x.RoomId == updatedReservation.RoomId &&
                x.Date == updatedReservation.Date &&
                updatedReservation.StartTime < x.EndTime &&
                updatedReservation.EndTime > x.StartTime);

        if (overlap)
            return Conflict("Time conflict");

        reservation.RoomId = updatedReservation.RoomId;
        reservation.OrganizerName = updatedReservation.OrganizerName;
        reservation.Topic = updatedReservation.Topic;
        reservation.Date = updatedReservation.Date;
        reservation.StartTime = updatedReservation.StartTime;
        reservation.EndTime = updatedReservation.EndTime;
        reservation.Status = updatedReservation.Status;

        return Ok(reservation);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var reservation = DataStore.Reservations
            .FirstOrDefault(x => x.Id == id);

        if (reservation == null)
            return NotFound();

        DataStore.Reservations.Remove(reservation);

        return NoContent();
    }
}