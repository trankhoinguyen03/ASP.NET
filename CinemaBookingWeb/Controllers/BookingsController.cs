﻿using Microsoft.AspNetCore.Mvc;
using CinemaBookingWeb.Data;
using CinemaBookingWeb.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CinemaBookingWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public void UpdateExpiredBookings()
        {
            var now = DateTime.Now;

            // Lấy danh sách các booking có showtimes đã qua và trạng thái là "Chưa thanh toán"
            var expiredBookings = _context.Bookings
                .Include(b => b.Showtime) // Bao gồm thông tin liên quan đến Showtime
                .Where(b => b.Showtime.StartTime < now && b.Status == 2) // Chỉ áp dụng cho trạng thái "Chưa thanh toán"
                .ToList();

            foreach (var booking in expiredBookings)
            {
                booking.Status = 0; // Chuyển trạng thái thành "Hủy"
            }

            _context.SaveChanges();
        }


        public IActionResult Index(string phoneNumber)
        {
            UpdateExpiredBookings(); // Cập nhật trạng thái các booking hết hạn

            var bookings = _context.Bookings
                .Include(b => b.Users)
                .AsQueryable();

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                bookings = bookings.Where(b => b.Users.Phone.Contains(phoneNumber));
            }

            return View(bookings.ToList());
        }

        public IActionResult BookingsDetails(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == id);
            if (booking == null)
            {
                return NotFound("Không tìm thấy đơn hàng.");
            }

            var bookingDetails = _context.BookingDetails
                .Where(bd => bd.BookingId == id)
                .ToList();

            var bookingCombos = _context.BookingCombos
                .Where(bc => bc.BookingId == id)
                .ToList();
            var statusList = new List<SelectListItem>
    {
        new SelectListItem { Value = "1", Text = "Đã thanh toán", Selected = booking.Status == 1 },
        new SelectListItem { Value = "2", Text = "Chưa thanh toán", Selected = booking.Status == 2 },
        new SelectListItem { Value = "0", Text = "Hủy", Selected = booking.Status == 0 }
    };
            var user = _context.Users.FirstOrDefault(u => u.UserId == booking.UserId);
            if (user == null)
            {
                return NotFound("Không tìm thấy thông tin người dùng.");
            }

            var viewModel = new BookingDetailViewModel
            {
                Booking = booking,
                StatusList = statusList,
                BookingDetails = bookingDetails,
                BookingCombos = bookingCombos,
                UserPhoneNumber = user.Phone,
            };

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult UpdateBookingStatus(int bookingId, byte status)
        {
            // Lấy thông tin booking từ cơ sở dữ liệu
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            // Cập nhật trạng thái của booking
            booking.Status = status;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();

            // Sau khi lưu thành công, chuyển hướng về trang danh sách các đơn hàng (Index)
            return RedirectToAction("Index");
        }



    }
}
