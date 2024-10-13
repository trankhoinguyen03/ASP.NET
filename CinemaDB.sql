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
	Price DECIMAL(10, 2) NOT NULL,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NOT NULL,
    Hall NVARCHAR(50),
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId),
    FOREIGN KEY (CinemaId) REFERENCES Cinemas(CinemaId)
);

-- Tạo bảng Seats (Thông tin ghế ngồi)
CREATE TABLE Seats (
    SeatId INT PRIMARY KEY IDENTITY(1,1),
    SeatNumber NVARCHAR(10) NOT NULL,
    SeatType NVARCHAR(50),
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

-- Tạo bảng Combos (Thông tin combo bắp nước)
CREATE TABLE Combos (
    ComboId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(500),
    Price DECIMAL(10, 2) NOT NULL,
    Size NVARCHAR(50), -- Kích cỡ combo (Lớn, Vừa, Nhỏ)
    Type NVARCHAR(50) -- Loại combo (Ví dụ: Popcorn & Drink, Snack & Drink)
);

-- Tạo bảng BookingCombos (Thông tin combo đã được đặt)
CREATE TABLE BookingCombos (
    BookingComboId INT PRIMARY KEY IDENTITY(1,1),
    BookingId INT NOT NULL,
    ComboId INT NOT NULL,
    Quantity INT NOT NULL, -- Số lượng combo
    FOREIGN KEY (BookingId) REFERENCES Bookings(BookingId),
    FOREIGN KEY (ComboId) REFERENCES Combos(ComboId)
);

-- Thêm dữ liệu vào bảng Movies
INSERT INTO Movies (Title, Description, Duration, Rating, ReleaseDate, Genre, Language, ImageUrl)
VALUES 
('Avengers: Endgame', 'Siêu anh hùng chiến đấu chống lại Thanos.', 180, 'PG-13', '2019-04-26', 'Action', 'English', 'avengers.jpg'),
('Spider-Man: No Way Home', 'Spider-Man đối mặt với những thử thách mới.', 148, 'PG-13', '2021-12-17', 'Action', 'English', 'spiderman.jpg'),
('The Lion King', 'Hành trình của Simba để trở thành vua sư tử.', 118, 'G', '2019-07-19', 'Animation', 'English', 'lionking.jpg'),
('Frozen II', 'Anna và Elsa khám phá những bí ẩn về quá khứ của họ.', 103, 'PG', '2019-11-22', 'Animation', 'English', 'frozen2.jpg'),
('Joker', 'Câu chuyện về kẻ thù đáng sợ của Batman.', 122, 'R', '2019-10-04', 'Drama', 'English', 'joker.jpg'),
('Tenet', 'Một đặc vụ đi ngược thời gian để ngăn chặn Thế chiến III.', 150, 'PG-13', '2020-09-03', 'Sci-Fi', 'English', 'tenet.jpg'),
('Parasite', 'Hai gia đình với cuộc sống trái ngược nhau.', 132, 'R', '2019-05-30', 'Thriller', 'Korean', 'parasite.jpg'),
('The Matrix Resurrections', 'Trở lại với thế giới ảo Matrix.', 148, 'R', '2021-12-22', 'Sci-Fi', 'English', 'matrix.jpg'),
('Dune', 'Câu chuyện sử thi về hành tinh sa mạc.', 155, 'PG-13', '2021-10-22', 'Adventure', 'English', 'dune.jpg'),
('Black Widow', 'Câu chuyện về Natasha Romanoff.', 134, 'PG-13', '2021-07-09', 'Action', 'English', 'blackwidow.jpg');

-- Thêm dữ liệu vào bảng Cinemas
INSERT INTO Cinemas (Name, Location, Phone, City)
VALUES 
('Galaxy Nguyễn Du', '116 Nguyễn Du, Quận 1', '0123456789', 'Ho Chi Minh'),
('Galaxy Quang Trung', '304A Quang Trung, Gò Vấp', '0123456781', 'Ho Chi Minh'),
('Galaxy Kinh Dương Vương', '718 Kinh Dương Vương, Quận 6', '0123456782', 'Ho Chi Minh'),
('Galaxy Tân Bình', '246 Nguyễn Hồng Đào, Tân Bình', '0123456783', 'Ho Chi Minh'),
('Galaxy Cà Mau', '58 Lý Bôn, TP. Cà Mau', '0123456784', 'Cà Mau'),
('Galaxy Đà Nẵng', '79 Điện Biên Phủ, Thanh Khê', '0123456785', 'Đà Nẵng'),
('Galaxy Long Xuyên', '66 Trần Hưng Đạo, Long Xuyên', '0123456786', 'An Giang'),
('Galaxy Bình Dương', '555 Đại Lộ Bình Dương, Thủ Dầu Một', '0123456787', 'Bình Dương'),
('Galaxy Hà Nội', '76 Trần Duy Hưng, Cầu Giấy', '0123456788', 'Hà Nội'),
('Galaxy Phú Nhuận', '212 Phan Xích Long, Phú Nhuận', '0123456780', 'Ho Chi Minh');

-- Thêm dữ liệu vào bảng Showtimes
INSERT INTO Showtimes (MovieId, CinemaId, Price, StartTime, EndTime, Hall)
VALUES 
(1, 1, 90000, '2024-10-01 18:00', '2024-10-01 21:00', 'Hall 1'),
(2, 2, 90000, '2024-10-02 20:00', '2024-10-02 22:30', 'Hall 2'),
(3, 3, 90000, '2024-10-03 15:00', '2024-10-03 17:00', 'Hall 3'),
(4, 4, 90000, '2024-10-04 17:00', '2024-10-04 19:00', 'Hall 4'),
(5, 5, 90000, '2024-10-05 19:00', '2024-10-05 21:00', 'Hall 5'),
(6, 6, 90000, '2024-10-06 20:00', '2024-10-06 23:00', 'Hall 6'),
(7, 7, 90000, '2024-10-07 14:00', '2024-10-07 16:30', 'Hall 1'),
(8, 8, 90000, '2024-10-08 16:00', '2024-10-08 18:30', 'Hall 2'),
(9, 9, 90000, '2024-10-09 18:00', '2024-10-09 21:00', 'Hall 3'),
(10, 10, 90000, '2024-10-10 21:00', '2024-10-10 23:30', 'Hall 4');


-- Thêm dữ liệu vào bảng Seats
DECLARE @Row INT = 1;
DECLARE @SeatType NVARCHAR(50);
DECLARE @SeatNumber NVARCHAR(5);

WHILE @Row <= 8
BEGIN
    DECLARE @Column INT = 1;

    WHILE @Column <= 8
    BEGIN
        -- Tạo số ghế theo kiểu "A1", "A2", ..., "H8"
        SET @SeatNumber = CHAR(64 + @Row) + CAST(@Column AS NVARCHAR(2));

        -- Xác định loại ghế (VIP cho hàng > 5, Standard cho các hàng còn lại)
        IF @Row > 5
            SET @SeatType = 'VIP';
        ELSE
            SET @SeatType = 'Standard';

        -- Chèn vào bảng Seats
        INSERT INTO Seats (SeatNumber, SeatType)
        VALUES (@SeatNumber, @SeatType);

        SET @Column = @Column + 1;
    END;

    SET @Row = @Row + 1;
END;

-- Thêm dữ liệu vào bảng Users
INSERT INTO Users (Username, [Password], Email, Phone, Role)
VALUES 
('user1', 'password1', 'user1@example.com', '0123456789', 'User'),
('user2', 'password2', 'user2@example.com', '0123456781', 'User'),
('user3', 'password3', 'user3@example.com', '0123456782', 'User'),
('user4', 'password4', 'user4@example.com', '0123456783', 'User'),
('user5', 'password5', 'user5@example.com', '0123456784', 'User'),
('user6', 'password6', 'user6@example.com', '0123456785', 'User'),
('user7', 'password7', 'user7@example.com', '0123456786', 'Admin'),
('user8', 'password8', 'user8@example.com', '0123456787', 'User'),
('user9', 'password9', 'user9@example.com', '0123456788', 'User'),
('user10', 'password10', 'user10@example.com', '0123456780', 'Admin');


-- Thêm dữ liệu vào bảng Bookings
INSERT INTO Bookings (UserId, ShowtimeId, BookingDate, TotalPrice)
VALUES 
(1, 1, GETDATE(), 150000),
(2, 2, GETDATE(), 200000),
(3, 3, GETDATE(), 100000),
(4, 4, GETDATE(), 120000),
(5, 5, GETDATE(), 180000),
(6, 6, GETDATE(), 160000),
(7, 7, GETDATE(), 130000),
(8, 8, GETDATE(), 170000),
(9, 9, GETDATE(), 140000),
(10, 10, GETDATE(), 190000);


-- Thêm dữ liệu vào bảng BookingDetails
INSERT INTO BookingDetails (BookingId, SeatId, Price)
VALUES 
(1, 1, 75000),
(1, 2, 75000),
(2, 3, 100000),
(2, 4, 100000),
(3, 5, 50000),
(3, 6, 50000),
(4, 7, 60000),
(4, 8, 60000),
(5, 9, 90000),
(5, 10, 90000);

-- Thêm dữ liệu vào bảng Combos
INSERT INTO Combos (Name, Description, Price, Size, Type)
VALUES 
('Combo Lớn', '1 bắp lớn, 2 nước ngọt lớn.', 120000, 'Lớn', 'Popcorn & Drink'),
('Combo Vừa', '1 bắp vừa, 1 nước ngọt vừa.', 90000, 'Vừa', 'Popcorn & Drink'),
('Combo Nhỏ', '1 bắp nhỏ, 1 nước ngọt nhỏ.', 60000, 'Nhỏ', 'Popcorn & Drink'),
('Combo Gia đình', '2 bắp lớn, 4 nước ngọt lớn.', 200000, 'Lớn', 'Popcorn & Drink'),
('Combo Snack', '1 snack lớn, 1 nước ngọt lớn.', 80000, 'Lớn', 'Snack & Drink'),
('Combo Đặc biệt', '1 bắp lớn, 2 snack lớn, 2 nước ngọt.', 150000, 'Lớn', 'Popcorn & Snack'),
('Combo Trẻ em', '1 bắp nhỏ, 1 nước trái cây.', 50000, 'Nhỏ', 'Popcorn & Juice'),
('Combo Cặp đôi', '1 bắp lớn, 2 nước ngọt.', 110000, 'Lớn', 'Popcorn & Drink'),
('Combo Người lớn', '1 bắp lớn, 1 nước ngọt, 1 bia.', 130000, 'Lớn', 'Popcorn & Beer'),
('Combo Snack Mix', '1 bắp, 1 snack, 1 nước ngọt.', 90000, 'Vừa', 'Popcorn & Snack');


-- Thêm dữ liệu vào bảng BookingCombos
INSERT INTO BookingCombos (BookingId, ComboId, Quantity)
VALUES 
(1, 1, 2),
(2, 2, 1),
(3, 3, 3),
(4, 4, 2),
(5, 5, 1),
(6, 6, 2),
(7, 7, 1),
(8, 8, 1),
(9, 9, 2),
(10, 10, 1);

