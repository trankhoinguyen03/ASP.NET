-- Tạo cơ sở dữ liệu
CREATE DATABASE CinemaDB;
GO

-- Sử dụng cơ sở dữ liệu vừa tạo
USE CinemaDB;
GO

-- Tạo bảng Movies (Thông tin phim)
CREATE TABLE Movies (
    MovieId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,
    Description TEXT,
    Duration INT, -- thời lượng phim tính bằng phút
    Rating NVARCHAR(10),
    ReleaseDate DATE,
    Genre NVARCHAR(100),
    Language NVARCHAR(50),
    ImageUrl NVARCHAR(255)
);

-- Tạo bảng Cinemas (Thông tin rạp chiếu phim)
CREATE TABLE Cinemas (
    CinemaId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Location NVARCHAR(255),
    Phone NVARCHAR(50),
    City NVARCHAR(100)
);

-- Tạo bảng Showtimes (Thông tin suất chiếu)
CREATE TABLE Showtimes (
    ShowtimeId INT PRIMARY KEY IDENTITY(1,1),
    MovieId INT NOT NULL,
    CinemaId INT NOT NULL,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NOT NULL,
    Hall NVARCHAR(50),
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId),
    FOREIGN KEY (CinemaId) REFERENCES Cinemas(CinemaId)
);

-- Tạo bảng Seats (Thông tin ghế ngồi)
CREATE TABLE Seats (
    SeatId INT PRIMARY KEY IDENTITY(1,1),
    CinemaId INT NOT NULL,
    Hall NVARCHAR(50) NOT NULL,
    SeatNumber NVARCHAR(10) NOT NULL,
    SeatType NVARCHAR(50),
    FOREIGN KEY (CinemaId) REFERENCES Cinemas(CinemaId)
);

-- Tạo bảng Users (Thông tin người dùng)
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    [Password] NVARCHAR(255) NOT NULL, -- lưu trữ mật khẩu đã mã hóa
    Email NVARCHAR(255),
    Phone NVARCHAR(50),
    Role NVARCHAR(50) -- Vai trò người dùng (User/Admin)
);

-- Tạo bảng Bookings (Thông tin đặt vé)
CREATE TABLE Bookings (
    BookingId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    ShowtimeId INT NOT NULL,
    BookingDate DATETIME NOT NULL DEFAULT GETDATE(),
    TotalPrice DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (ShowtimeId) REFERENCES Showtimes(ShowtimeId)
);

-- Tạo bảng BookingDetails (Chi tiết đặt vé)
CREATE TABLE BookingDetails (
    BookingDetailId INT PRIMARY KEY IDENTITY(1,1),
    BookingId INT NOT NULL,
    SeatId INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (BookingId) REFERENCES Bookings(BookingId),
    FOREIGN KEY (SeatId) REFERENCES Seats(SeatId)
);
