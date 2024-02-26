import React from "react";
import fileservice from "../../services/file/fileservice";
import PropTypes from "prop-types";
import { useDropzone } from "react-dropzone";
import debug from "sabio-debug";
import { Form } from "react-bootstrap";

const FileUpload = ({ uploadComplete }) => {
  const _logger = debug.extend("FileUpload");

  const onDrop = (acceptedFiles) => {
    const formData = new FormData();
    acceptedFiles.forEach((file) => formData.append("files", file));

    fileservice
      .uploadFile(formData)
      .then(fileUploadSuccess)
      .catch(fileUploadError);
  };

  const { getRootProps, getInputProps } = useDropzone({
    onDrop,
  });

  const fileUploadSuccess = (response) => {
    _logger(
      "Successful Upload Payload from File Upload Component when calling fileUploadSuccess handler: ",
      response
    );
    uploadComplete(response);
  };

  const fileUploadError = (err) => {
    _logger("error uploading files: ", err);
  };
  function handleClick(event) {
    event.preventDefault();
    document.getElementById("fileInput").click();
  }
  return (
    <React.Fragment>
      <div>
        <Form>
          <Form.Group controlId="formFile" className="mb-3 mt-3">
            <Form.Control type="file" {...getInputProps()} />
            <button className="btn btn-info" onClick={handleClick}>
              Upload New File
            </button>
          </Form.Group>
        </Form>
        <div {...getRootProps()}>
          <input id="fileInput" {...getInputProps()} />
        </div>
      </div>
    </React.Fragment>
  );
};

FileUpload.propTypes = {
  uploadComplete: PropTypes.func,
};

export default FileUpload;
