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
('Avengers: Endgame', N'Siêu anh hùng chiến đấu chống lại Thanos.', 180, 'PG-13', '2019-04-26', 'Action', 'English', 'https://youtu.be/TcMBFSGVi1c?si=aJI5rSf5Cs7fQCTt', '/img/movies/avengers_endgame.jpg'),
('Spider-Man: No Way Home', N'Spider-Man đối mặt với những thử thách mới.', 148, 'PG-13', '2021-12-17', 'Action', 'English', 'https://youtu.be/rt-2cxAiPJk?si=0e8U9SRPjInJX2RN', '/img/movies/spiderman_nowayhome.jpg'),
('The Lion King', N'Hành trình của Simba để trở thành vua sư tử.', 118, 'G', '2019-07-19', 'Animation', 'English', 'https://youtu.be/vXvtBVidecc?si=wOxoEFgXtmZKtmbK', '/img/movies/thelionking.jpg'),
('Frozen II', N'Anna và Elsa khám phá những bí ẩn về quá khứ của họ.', 103, 'PG', '2019-11-22', 'Animation', 'English', 'https://youtu.be/Zi4LMpSDccc?si=4lPZSOTloo61Z9tG', '/img/movies/frozen2.jpg'),
('Joker', N'Câu chuyện về kẻ thù đáng sợ của Batman.', 122, 'R', '2019-10-04', 'Drama', 'English', 'https://youtu.be/t433PEQGErc?si=yoonP-juvLjuWnfZ', '/img/movies/joker.jpg'),
('Tenet', N'Một đặc vụ đi ngược thời gian để ngăn chặn Thế chiến III.', 150, 'PG-13', '2020-09-03', 'Sci-Fi', 'English', 'https://youtu.be/L3pk_TBkihU?si=g97E5K7OPaP_QSqV', '/img/movies/tenet.jpg'),
('Parasite', N'Hai gia đình với cuộc sống trái ngược nhau.', 132, 'R', '2019-05-30', 'Thriller', 'Korean', 'https://youtu.be/SEUXfv87Wpk?si=LmcnD-SJezpX0uoL', '/img/movies/parasite.jpg'),
('The Matrix Resurrections', N'Trở lại với thế giới ảo Matrix.', 148, 'R', '2021-12-22', 'Sci-Fi', 'English', 'https://youtu.be/9ix7TUGVYIo?si=q4MU-1BBfCKuBpXY', '/img/movies/thematrix.jpg'),
('Dune', N'Câu chuyện sử thi về hành tinh sa mạc.', 155, 'PG-13', '2021-10-22', 'Adventure', 'English', 'https://youtu.be/n9xhJrPXop4?si=WVR_DXpce3eNUKDi', '/img/movies/dune.jpg'),
('Black Widow', N'Câu chuyện về Natasha Romanoff.', 134, 'PG-13', '2021-07-09', 'Action', 'English', 'https://youtu.be/ybji16u608U?si=NK4IrLm_4XlhJZ-f', '/img/movies/blackwidow.jpg');

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
(1, 1, 90000, '2024-12-09 18:00', '2024-12-09 21:00', 'Phòng 1.A'),
(2, 2, 90000, '2024-12-09 18:00', '2024-12-09 21:00', 'Phòng 1.B'),
(3, 3, 90000, '2024-12-09 18:00', '2024-12-09 21:00', 'Phòng 1.C'),
(4, 4, 90000, '2024-12-09 18:00', '2024-12-09 21:00', 'Phòng 2.A'),
(5, 5, 90000, '2024-12-09 18:00', '2024-12-09 21:00', 'Phòng 2.B'),
(6, 6, 90000, '2024-12-09 18:00', '2024-12-09 21:00', 'Phòng 2.C'),
(7, 7, 90000, '2024-12-09 18:00', '2024-12-09 21:00', 'Phòng 3.A'),
(8, 8, 90000, '2024-12-09 18:00', '2024-12-09 21:00', 'Phòng 3.B'),
(9, 9, 90000, '2024-12-09 18:00', '2024-12-09 21:00', 'Phòng 3.C'),
(10, 10, 90000, '2024-12-09 18:00', '2024-12-09 21:00', 'Phòng 3.D');


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
(1, 1, '2024-12-8 18:00', 150000),
(2, 2, '2024-12-8 18:00', 200000),
(3, 3, '2024-12-8 18:00', 100000),
(4, 4, '2024-12-8 18:00', 120000),
(5, 5, '2024-12-8 18:00', 180000),
(6, 6, '2024-12-8 18:00', 160000),
(7, 7, '2024-12-8 18:00', 130000),
(8, 8, '2024-12-8 18:00', 170000),
(9, 9, '2024-12-8 18:00', 140000),
(10, 10, '2024-12-8 18:00', 190000);


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
(5, 10, 90000),
(6, 9, 90000),
(7, 10, 90000);

-- Thêm dữ liệu vào bảng Combos
INSERT INTO Combos ([Name], [Description], Price, Size, [Type], ImageUrl)
VALUES 
(N'Combo Lớn', N'1 bắp lớn, 2 nước ngọt lớn, 1 snack lớn.', 200000, N'Lớn', 'Popcorn & Drink & Snack', '\img\combos\1bap_2nuoc_1snack.jpg'),
(N'Combo Vừa', N'1 bắp vừa, 1 nước ngọt vừa, 1 snack vừa.', 13000, N'Vừa', 'Popcorn & Drink & Snack', '\img\combos\1bap_1nuoc_1snack.jpg'),
(N'Combo Nhỏ', N'1 bắp nhỏ, 1 nước ngọt nhỏ.', 50000, N'Nhỏ', 'Popcorn & Drink', '\img\combos\1bap_1nuoc.jpg'),
(N'Combo Vừa', N'1 bắp vừa, 2 nước ngọt vừa.', 90000, N'Vừa', 'Popcorn & Drink', '\img\combos\1bap_2nuoc.jpg');


-- Thêm dữ liệu vào bảng BookingCombos
INSERT INTO BookingCombos (BookingId, ComboId, Quantity)
VALUES 
(1, 1, 2),
(2, 2, 1),
(3, 3, 3),
(4, 4, 2),
(5, 1, 1),
(6, 2, 2),
(7, 3, 1),
(8, 4, 1),
(9, 1, 2),
(10, 2, 1);

-- Tạo bảng Banner (Thông tin Banners)
CREATE TABLE Banner (
    BannerId INT IDENTITY(1,1) PRIMARY KEY,
    ImageUrl NVARCHAR(MAX) NOT NULL,
    Category NVARCHAR(MAX) NOT NULL
);
INSERT INTO Banner(ImageUrl, Category)
VALUES
('/img/banners/cbfa76d3-a46a-4c2e-b0d3-baaae101f799.jpg','Main'),
('/img/banners/7c3075f7-9f3f-464f-b17d-bce87303243a.jpg', 'Main'),
('/img/banners/1600e54a-a791-438c-bd6e-473dbcddf5f8.jpg', 'Main'),
('/img/banners/952552e3-5e90-459d-b445-27c7a1ef1b26.jpg', 'Cinema'),
('/img/banners/238b6f91-955a-4b87-9529-125445e33f2a.jpg', 'Cinema'),
('/img/banners/bf752ffc-a72e-4736-b5f7-dc8657b4f378.jpg', 'Cinema');
