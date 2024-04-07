$(document).ready(function () {
  // Function to populate shipping lines select box
  function populateShippingLines() {
    axios
      .get("http://localhost:5062/api/CangNhapKhau/hang-tau")
      .then(function (response) {
        $("#shippingLine").empty();
        $("#shippingLine").append(
          $("<option>", {
            value: "",
            text: "--Chọn--",
          })
        );
        response.data.data.forEach(function (item) {
          $("#shippingLine").append(
            $("<option>", {
              value: item,
              text: item,
            })
          );
        });
      })
      .catch(function (error) {
        console.error("Error fetching shipping lines:", error);
      });
  }

  // Call the function to populate shippingLine select box on page load
  populateShippingLines();

  // Handle form submission for Cảng Nhập Khẩu
  $("#filterForm").submit(function (event) {
    // Prevent default form submission behavior
    event.preventDefault();

    // Get values from form inputs
    var shippingLine = $("#shippingLine").val();
    var cargoType = $("#cargoType").val();
    var containerType = $("#containerType").val();

    // Convert cargoType to numeric value as per your specification
    var cargoTypeNumeric;
    switch (cargoType) {
      case "Bao":
        cargoTypeNumeric = 0;
        break;
      case "Xá":
        cargoTypeNumeric = 1;
        break;
      case "Cont":
        cargoTypeNumeric = 2;
        break;
      default:
        cargoTypeNumeric = "";
        break;
    }

    // Build API URL with parameters
    var apiUrl = "http://localhost:5062/api/CangNhapKhau";
    var params = {
      HangTau: shippingLine,
      LoaiHang: cargoTypeNumeric,
      LoaiCont: containerType,
    };
    axios
      .get(apiUrl, { params: params })
      .then(function (response) {
        console.log("API response:", response.data);
        // Handle response data
        fillCangNhapKhauTable(response);
        globalCangNhapKhauData = response.data;
      })
      .catch(function (error) {
        console.error("API error:", error);
        // Handle API error
        $("#dataTable")
          .empty()
          .append("<tr><td colspan='3'>API error</td></tr>");
      });
  });

  // Function to fill Cảng Nhập Khẩu table with data
  function fillCangNhapKhauTable(response) {
    var data = response.data.data;
    var tableBody = $("#dataTable tbody");
    tableBody.empty(); // Clear existing rows
    if (data && data.length > 0) {
      data.forEach(function (item) {
        var row = $("<tr>");
        row.append($("<td>").text(item.tenPhi));
        row.append($("<td>").text(item.donGia.toLocaleString())); // Format number with commas
        row.append($("<td>").text(item.donViTinh));
        tableBody.append(row);
      });
    } else {
      // No data, display message
      tableBody.empty(); // Clear existing rows
      tableBody.append("<tr><td colspan='3'>No data</td></tr>");
    }
  }
});
