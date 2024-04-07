// Function to calculate VAT
function calculateVAT(total) {
  return total * 0.08;
}

// Function to populate summary table
function populateSummaryTable() {
  var summaryTableBody = $("#summaryTable tbody");
  summaryTableBody.empty(); // Clear existing rows

  var total = 0;

  // Check if globalCangNhapKhauData and globalKhaiThacCangData are defined and contain data
  if (
    globalCangNhapKhauData &&
    globalCangNhapKhauData.data &&
    globalKhaiThacCangData &&
    globalKhaiThacCangData.data
  ) {
    // Loop through globalCangNhapKhauData to calculate total and add rows
    globalCangNhapKhauData.data.forEach(function (item) {
      var row = $("<tr>");
      row.append($("<td>").text(item.tenPhi));
      row.append($("<td>").text(item.donGia.toLocaleString()));
      row.append($("<td>").text(item.donViTinh));
      summaryTableBody.append(row);
      total += item.donGia;
    });

    // Loop through globalKhaiThacCangData to calculate total and add rows
    globalKhaiThacCangData.data.forEach(function (item) {
      var row = $("<tr>");
      row.append($("<td>").text(item.tenPhi));
      row.append($("<td>").text(item.donGia.toLocaleString()));
      row.append($("<td>").text(item.donViTinh));
      summaryTableBody.append(row);
      total += item.donGia;
    });

    // Calculate VAT
    var vat = calculateVAT(total);

    // Add total, VAT, and total with VAT rows
    summaryTableBody.append(
      "<><tr><td colspan='3'><strong>Tổng: </strong>" +
        total.toLocaleString() +
        "</td></tr>"
    );
    summaryTableBody.append(
      "<tr><td colspan='3'><strong>VAT: </strong>" +
        vat.toLocaleString() +
        "</td></tr>"
    );
    summaryTableBody.append(
      "<tr><td colspan='3'><strong>Tổng đã VAT: </strong>" +
        (total + vat).toLocaleString() +
        "</td></tr>"
    );
  } else {
    // Handle case when data is not available
    summaryTableBody.append(
      "<tr><td colspan='3'>Dữ liệu không có sẵn.</td></tr>"
    );
  }
}
