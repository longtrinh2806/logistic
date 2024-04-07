$(document).ready(function () {
  // Function to populate dropdown options from API
  function populateDropdown(apiUrl, dropdownId) {
    axios
      .get(apiUrl)
      .then(function (response) {
        $(dropdownId).empty();
        $(dropdownId).append(
          $("<option>", {
            value: "",
            text: "--Chọn--",
          })
        );
        response.data.data.forEach(function (item) {
          $(dropdownId).append(
            $("<option>", {
              value: item,
              text: item,
            })
          );
        });
      })
      .catch(function (error) {
        console.error("Error fetching dropdown options:", error);
      });
  }

  // Populate dropdowns for Khai Thác Cảng
  populateDropdown(
    "http://localhost:5062/api/KhaiThacCang/cang-do",
    "#portUnloading"
  );
  populateDropdown(
    "http://localhost:5062/api/KhaiThacCang/don-vi-khai-thac",
    "#operatingUnit"
  );
  populateDropdown(
    "http://localhost:5062/api/KhaiThacCang/phuong-tien",
    "#receivingVehicle"
  );
  populateDropdown(
    "http://localhost:5062/api/KhaiThacCang/phuong-thuc-giao-nhan",
    "#deliveryMethod"
  );

  // Handle form submission for Khai Thác Cảng
  $("#portOperationFilterForm").submit(function (event) {
    // Prevent default form submission behavior
    event.preventDefault();

    // Get values from form inputs
    var portUnloading = $("#portUnloading").val();
    var operatingUnit = $("#operatingUnit").val();
    var receivingVehicle = $("#receivingVehicle").val();
    var deliveryMethod = $("#deliveryMethod").val();

    // Build API URL with parameters
    var apiUrl = "http://localhost:5062/api/KhaiThacCang";
    var params = {
      CangDo: portUnloading,
      DonViKhaiThac: operatingUnit,
      PhuongTienNhanHang: receivingVehicle,
      PhuongThucGiaoNhan: deliveryMethod,
    };
    axios
      .get(apiUrl, { params: params })
      .then(function (response) {
        console.log("API response:", response.data);
        // Handle response data
        fillKhaiThacCangTable(response);
        globalKhaiThacCangData = response.data;
        populateSummaryTable();
      })
      .catch(function (error) {
        console.error("API error:", error);
        // Handle API error
      });
  });

  // Function to fill Khai Thác Cảng table with data
  function fillKhaiThacCangTable(response) {
    var data = response.data.data;
    var tableBody = $("#portOperationTable tbody");
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
