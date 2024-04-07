function uploadFile() {
  const file = document.getElementById("fileInput").files[0];
  if (file) {
    const formData = new FormData();
    formData.append("file", file);

    axios({
      method: "post",
      url: "http://localhost:5062/api/Excel",
      data: formData,
      headers: {
        "Content-Type": "multipart/form-data",
      },
    })
      .then((response) => {
        console.log("API response:", response.data);
        if (response.data.succeeded) {
          console.log("Upload succeeded.");
          // Xử lý tệp thành công
          alert("Upload thành công!");
        } else {
          console.warn("Upload failed:", response.data.message);
          // Xử lý tệp thất bại
          alert("Upload thất bại: " + response.data.message);
        }
      })
      .catch((error) => {
        console.error("API error:", error.response.data.message);
        // Xử lý lỗi từ API
        alert("Lỗi API: " + error.response.data.message);
      });
  }
}
