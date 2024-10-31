-- Tạo cơ sở dữ liệu
CREATE DATABASE CinemaDB;
GO

-- Sử dụng cơ sở dữ liệu vừa tạo
USE CinemaDB;
GO

-- Hiển thị tiếng Việt
ALTER DATABASE CinemaDB COLLATE Vietnamese_CI_AS;
GO

-- Tạo bảng Movies (Thông tin phim)
CREATE TABLE Movies (
    MovieId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(1000),
    Duration INT, -- thời lượng phim tính bằng phút
    Rating NVARCHAR(10),
    ReleaseDate DATE,
    Genre NVARCHAR(100),
    [Language] NVARCHAR(50),
	TrailerUrl NVARCHAR(255),
    ImageUrl NVARCHAR(255),
	[Status] TINYINT NOT NULL DEFAULT 1
);

-- Tạo bảng Cinemas (Thông tin rạp chiếu phim)
CREATE TABLE Cinemas (
    CinemaId INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(255) NOT NULL,
    [Location] NVARCHAR(255),
    Phone NVARCHAR(50),
    City NVARCHAR(100),
	[Status] TINYINT NOT NULL DEFAULT 1
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
	[Status] TINYINT NOT NULL DEFAULT 1
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId),
    FOREIGN KEY (CinemaId) REFERENCES Cinemas(CinemaId),
);

-- Tạo bảng Seats (Thông tin ghế ngồi)
CREATE TABLE Seats (
    SeatId INT PRIMARY KEY IDENTITY(1,1),
    SeatNumber NVARCHAR(10) NOT NULL,
    SeatType NVARCHAR(50),
	[Status] TINYINT NOT NULL DEFAULT 1
);

-- Tạo bảng Users (Thông tin người dùng)
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(100) NOT NULL UNIQUE,
    [Password] NVARCHAR(255) NOT NULL, -- lưu trữ mật khẩu đã mã hóa
    Email NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(50) NOT NULL,
	SignupDate DATETIME,
    [Role] NVARCHAR(50) NOT NULL DEFAULT 'User', -- Vai trò người dùng (User/Admin)
	[Status] TINYINT NOT NULL DEFAULT 1
);

-- Tạo bảng Bookings (Thông tin đặt vé)
CREATE TABLE Bookings (
    BookingId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    ShowtimeId INT NOT NULL,
    BookingDate DATETIME NOT NULL DEFAULT GETDATE(),
    TotalPrice DECIMAL(10, 2) NOT NULL,
	[Status] TINYINT NOT NULL DEFAULT 1,
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (ShowtimeId) REFERENCES Showtimes(ShowtimeId)
);

-- Tạo bảng BookingDetails (Chi tiết đặt vé)
CREATE TABLE BookingDetails (
    BookingDetailId INT PRIMARY KEY IDENTITY(1,1),
    BookingId INT NOT NULL,
    SeatId INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
	[Status] TINYINT NOT NULL DEFAULT 1,
    FOREIGN KEY (BookingId) REFERENCES Bookings(BookingId),
    FOREIGN KEY (SeatId) REFERENCES Seats(SeatId)
);

-- Tạo bảng Combos (Thông tin combo bắp nước)
CREATE TABLE Combos (
    ComboId INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(500),
    Price DECIMAL(10, 2) NOT NULL,
    Size NVARCHAR(50), -- Kích cỡ combo (Lớn, Vừa, Nhỏ)
    [Type] NVARCHAR(50), -- Loại combo (Ví dụ: Popcorn & Drink, Snack & Drink)
	ImageUrl NVARCHAR(255),
	[Status] TINYINT NOT NULL DEFAULT 1
);

-- Tạo bảng BookingCombos (Thông tin combo đã được đặt)
CREATE TABLE BookingCombos (
    BookingComboId INT PRIMARY KEY IDENTITY(1,1),
    BookingId INT NOT NULL,
    ComboId INT NOT NULL,
    Quantity INT NOT NULL, -- Số lượng combo
	[Status] TINYINT NOT NULL DEFAULT 1,
    FOREIGN KEY (BookingId) REFERENCES Bookings(BookingId),
    FOREIGN KEY (ComboId) REFERENCES Combos(ComboId),
);

-- Thêm dữ liệu vào bảng Movies
INSERT INTO Movies (Title, [Description], Duration, Rating, ReleaseDate, Genre, [Language], TrailerUrl, ImageUrl)
VALUES 
('Avengers: Endgame', N'Siêu anh hùng chiến đấu chống lại Thanos.', 180, 'PG-13', '2019-04-26', 'Action', 'English', 'url', 'avengers.jpg'),
('Spider-Man: No Way Home', N'Spider-Man đối mặt với những thử thách mới.', 148, 'PG-13', '2021-12-17', 'Action', 'English', 'url', 'spiderman.jpg'),
('The Lion King', N'Hành trình của Simba để trở thành vua sư tử.', 118, 'G', '2019-07-19', 'Animation', 'English', 'url', 'lionking.jpg'),
('Frozen II', N'Anna và Elsa khám phá những bí ẩn về quá khứ của họ.', 103, 'PG', '2019-11-22', 'Animation', 'English', 'url', 'frozen2.jpg'),
('Joker', N'Câu chuyện về kẻ thù đáng sợ của Batman.', 122, 'R', '2019-10-04', 'Drama', 'English', 'url', 'joker.jpg'),
('Tenet', N'Một đặc vụ đi ngược thời gian để ngăn chặn Thế chiến III.', 150, 'PG-13', '2020-09-03', 'Sci-Fi', 'English', 'url', 'tenet.jpg'),
('Parasite', N'Hai gia đình với cuộc sống trái ngược nhau.', 132, 'R', '2019-05-30', 'Thriller', 'Korean', 'url', 'parasite.jpg'),
('The Matrix Resurrections', N'Trở lại với thế giới ảo Matrix.', 148, 'R', '2021-12-22', 'Sci-Fi', 'English', 'url', 'matrix.jpg'),
('Dune', N'Câu chuyện sử thi về hành tinh sa mạc.', 155, 'PG-13', '2021-10-22', 'Adventure', 'English', 'url', 'dune.jpg'),
('Black Widow', N'Câu chuyện về Natasha Romanoff.', 134, 'PG-13', '2021-07-09', 'Action', 'English', 'url', 'blackwidow.jpg');

-- Thêm dữ liệu vào bảng Cinemas
INSERT INTO Cinemas ([Name], [Location], Phone, City)
VALUES 
(N'Galaxy Nguyễn Du', N'116 Nguyễn Du, Quận 1', '0123456789', N'Thành phố Hồ Chí Minh'),
(N'Galaxy Quang Trung', N'304A Quang Trung, Gò Vấp', '0123456781', N'Thành phố Hồ Chí Minh'),
(N'Galaxy Kinh Dương Vương', N'718 Kinh Dương Vương, Quận 6', '0123456782', N'Thành phố Hồ Chí Minh'),
(N'Galaxy Tân Bình', N'246 Nguyễn Hồng Đào, Tân Bình', '0123456783', N'Thành phố Hồ Chí Minh'),
(N'Galaxy Cà Mau', N'58 Lý Bôn, TP. Cà Mau', '0123456784', N'Cà Mau'),
(N'Galaxy Đà Nẵng', N'79 Điện Biên Phủ, Thanh Khê', '0123456785', N'Đà Nẵng'),
(N'Galaxy Long Xuyên', N'66 Trần Hưng Đạo, Long Xuyên', '0123456786', N'An Giang'),
(N'Galaxy Bình Dương', N'555 Đại Lộ Bình Dương, Thủ Dầu Một', '0123456787', N'Bình Dương'),
(N'Galaxy Hà Nội', N'76 Trần Duy Hưng, Cầu Giấy', '0123456788', N'Hà Nội'),
(N'Galaxy Phú Nhuận', N'212 Phan Xích Long, Phú Nhuận', '0123456780', N'Thành phố Hồ Chí Minh');

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
INSERT INTO Users (UserName, [Password], Email, Phone, SignupDate, [Role])
VALUES 
('user1', 'password1', 'user1@example.com', '0123456789', '2024-10-01 21:00', 'User'),
('user2', 'password2', 'user2@example.com', '0123456781', '2024-10-01 21:00', 'User'),
('user3', 'password3', 'user3@example.com', '0123456782', '2024-10-01 21:00', 'User'),
('user4', 'password4', 'user4@example.com', '0123456783', '2024-10-01 21:00', 'User'),
('user5', 'password5', 'user5@example.com', '0123456784', '2024-10-01 21:00', 'User'),
('user6', 'password6', 'user6@example.com', '0123456785', '2024-10-01 21:00', 'User'),
('user7', 'password7', 'user7@example.com', '0123456786', '2024-10-01 21:00', 'Admin'),
('user8', 'password8', 'user8@example.com', '0123456787', '2024-10-01 21:00', 'User'),
('user9', 'password9', 'user9@example.com', '0123456788', '2024-10-01 21:00', 'User'),
('user10', 'password10', 'user10@example.com', '0123456780', '2024-10-01 21:00', 'Admin');


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
INSERT INTO Combos ([Name], [Description], Price, Size, [Type], ImageUrl)
VALUES 
(N'Combo Lớn', N'1 bắp lớn, 2 nước ngọt lớn.', 120000, N'Lớn', 'Popcorn & Drink', 'combo.jpg'),
(N'Combo Vừa', N'1 bắp vừa, 1 nước ngọt vừa.', 90000, N'Vừa', 'Popcorn & Drink', 'combo.jpg'),
(N'Combo Nhỏ', N'1 bắp nhỏ, 1 nước ngọt nhỏ.', 60000, N'Nhỏ', 'Popcorn & Drink', 'combo.jpg'),
(N'Combo Gia đình', N'2 bắp lớn, 4 nước ngọt lớn.', 200000, N'Lớn', 'Popcorn & Drink', 'combo.jpg'),
(N'Combo Snack', N'1 snack lớn, 1 nước ngọt lớn.', 80000, N'Lớn', 'Snack & Drink', 'combo.jpg'),
(N'Combo Đặc biệt', N'1 bắp lớn, 2 snack lớn, 2 nước ngọt.', 150000, N'Lớn', 'Popcorn & Snack', 'combo.jpg'),
(N'Combo Trẻ em', N'1 bắp nhỏ, 1 nước trái cây.', 50000, N'Nhỏ', 'Popcorn & Juice', 'combo.jpg'),
(N'Combo Cặp đôi', N'1 bắp lớn, 2 nước ngọt.', 110000, N'Lớn', 'Popcorn & Drink', 'combo.jpg'),
(N'Combo Người lớn', N'1 bắp lớn, 1 nước ngọt, 1 bia.', 130000, N'Lớn', 'Popcorn & Beer', 'combo.jpg'),
(N'Combo Snack Mix', N'1 bắp, 1 snack, 1 nước ngọt.', 90000, N'Vừa', 'Popcorn & Snack', 'combo.jpg');


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

