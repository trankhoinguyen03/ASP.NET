# Đồ án ASP.NET
## Website đăng ký vé xem phim
_Thành viên: 6_

----

[Web tham khảo cho phần admin](https://techmaster.vn/posts/37941/gioi-thieu-du-an-web-dat-ve-xem-phim-truc-tuyen-chuc-nang-admin)

[Web tham khảo cho phần user](https://www.galaxycine.vn/booking/)

----
### Danh sách công việc

| Admin | User  |
| --------      | --------      |
| (Số người: 4) | (Số người: 2) |

#### Chức năng của Admin:

![Admin Home Page](https://github.com/user-attachments/assets/c42a19e5-bb13-4f40-a1f3-0af68a9b2b4f)


##### 1. Xem tổng quan các thông tin của trang web
_Admin có thể xem tổng quan các thông tin quan trọng như khách hàng mới, tổng số vé bán ra, doanh thu theo phim, doanh thu theo rạp chiếu phim và tổng doanh thu. Giao diện tổng quan cung cấp một cái nhìn toàn diện và chi tiết về hoạt động kinh doanh của trang web._
<br>
- Khách hàng mới: Thống kê số lượng khách hàng mới đăng ký theo ngày và theo tháng.
- Tổng số vé bán ra: Tổng hợp số vé đã bán theo ngày và theo tháng.
- Doanh thu theo phim: Thống kê doanh thu của từng bộ phim, giúp xác định các phim bán chạy nhất.
- Doanh thu theo rạp chiếu phim: Thống kê doanh thu của từng rạp, giúp đánh giá hiệu suất của từng địa điểm chiếu phim.
- Tổng doanh thu: Hiển thị doanh thu theo ngày và theo tháng, giúp admin đánh giá hiệu suất kinh doanh theo thời gian.

##### 2. Quản lý phim
_Admin có thể thực hiện các thao tác như thêm phim mới, sửa thông tin phim, xóa phim và xem danh sách tất cả các phim hiện có trên hệ thống._
<br>
- Thêm phim: Admin điền các thông tin cần thiết về phim như tên phim, thể loại, đạo diễn, diễn viên, ngày phát hành, thời lượng, và mô tả phim. Hình ảnh và trailer phim cũng có thể được tải lên để người xem có thể tham khảo.
- Sửa phim: Admin có thể chỉnh sửa các thông tin của phim nếu có sự thay đổi hoặc bổ sung thông tin mới.
- Danh sách phim: Hiển thị danh sách tất cả các phim có trên hệ thống cùng với các thông tin cơ bản, giúp admin dễ dàng quản lý và theo dõi.

##### 3. Quản lý rạp chiếu phim
_Admin có thể thêm mới, sửa đổi, xóa bỏ và xem danh sách các rạp chiếu phim trong hệ thống._
<br>
- Thêm rạp chiếu phim: Admin điền các thông tin cần thiết về rạp như tên rạp, địa chỉ, số điện thoại liên hệ và các thông tin khác liên quan.
- Sửa rạp chiếu phim: Admin có thể chỉnh sửa các thông tin của rạp chiếu nếu có sự thay đổi hoặc bổ sung thông tin mới.
- Danh sách rạp chiếu phim: Hiển thị danh sách tất cả các rạp có trên hệ thống cùng với các thông tin cơ bản, giúp admin dễ dàng quản lý và theo dõi.

##### 4. Quản lý suất chiếu
_Admin có thể quản lý các suất chiếu bằng cách thêm mới, sửa đổi, xóa bỏ và xem danh sách các suất chiếu._
<br>
- Thêm suất chiếu: Admin điền các thông tin cần thiết về suất chiếu như giờ bắt đầu, giờ kết thúc, giá suất chiếu, và các thông tin liên quan khác.
- Sửa suất chiếu: Admin có thể chỉnh sửa các thông tin của suất chiếu nếu có sự thay đổi hoặc bổ sung thông tin mới.
- Danh sách suất chiếu: Hiển thị danh sách tất cả các suất chiếu có trên hệ thống cùng với các thông tin cơ bản, giúp admin dễ dàng quản lý và theo dõi.

##### 5. Quản lý đơn hàng
_Admin có thể xem danh sách đơn hàng, xem chi tiết từng đơn hàng, xác nhận đơn hàng và hủy đơn hàng nếu cần._
<br>
- Danh sách đơn hàng: Hiển thị danh sách tất cả các đơn hàng đã được đặt trên hệ thống cùng với các thông tin cơ bản như mã đơn hàng, tên khách hàng, và trạng thái đơn hàng.
- Chi tiết đơn hàng: Admin có thể xem chi tiết từng đơn hàng bao gồm thông tin vé, dịch vụ đi kèm, và tổng số tiền.

##### 6. Quản lý người dùng
_Admin có thể xem danh sách người dùng, xem chi tiết, sửa thông tin và xóa người dùng nếu cần._
<br>
- Danh sách người dùng: Hiển thị danh sách tất cả các người dùng đã đăng ký trên hệ thống cùng với các thông tin cơ bản như tên, email, và trạng thái tài khoản.
- Sửa thông tin người dùng: Admin có thể chỉnh sửa thông người dùng nếu cần thiết.
- Chi tiết người dùng: Admin có thể xem chi tiết thông tin của từng người dùng bao gồm thông tin cá nhân, lịch sử đặt vé, và các hoạt động liên quan.
- Thêm người dùng: Admin điền các thông tin cần thiết về người dùng như tên, email, số điện thoại và các thông tin liên quan khác.

##### 7. Quản lý combo
_Admin có thể xem danh sách combo, xem chi tiết, thêm sửa thông tin và xóa combo nếu cần._
<br>
- Danh sách combo: Hiển thị danh sách tất cả combo
- Sửa thông tin combo: Admin có thể chỉnh sửa thông tin combo nếu có sự thay đổi hoặc bổ sung thông tin mới.
- Thêm combo: Admin điền các thông tin combo như tên, giá cả, hình ảnh...

----

#### Chức năng của User:
##### 1. Đăng ký
_User có thể đăng ký tài khoản với các thông tin cơ bản như tên, số điện thoại, email... để có thể đăng nhập và sử dụng chức năng của trang web._

##### 2. Đăng nhập
_User có thể dùng tài khoản đã đăng ký để đăng nhập vào trang web và sử dụng các chức năng của trang web._

##### 3. Đặt vé
_User có thể sử dụng chức năng đặt vé bằng cách chọn thành phố, rạp chiếu phim, phim, suất chiếu và combo sau đó chọn thanh toán._

----

### Onion Architecture
Domain <- Application <- Infrastructure
<br>
Application & Infrastructure <- Presentation