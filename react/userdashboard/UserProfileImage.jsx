import React from "react";
import FileUpload from "components/files/FileUpload";
import PropTypes from "prop-types";
import debug from "sabio-debug";
import "../userdashboard/userDashboardStyling.css";

const _logger = debug.extend("UserProfileImage");

const UserProfileImage = (props) => {
  _logger("User Avatar Url: ", props.userData.avatarUrl);

  function onUploadSuccess(response) {
    props.handleUploadComplete(response);
    _logger(
      "Success from UserProfileImage upon calling onUploadSuccess: ",
      response.items
    );
  }

  return (
    <div className="col-12">
      <div className="card mb-4">
        <div className="card-header align-items-center card-header-height d-flex justify-content-between align-items-center">
          <div>
            <h2 className="mb-0">User Profile Image</h2>
          </div>
        </div>
        <div className="user card-body">
          <div className="avatar-container">
            <img
              src={props.userData.avatarUrl}
              alt="User Avatar"
              className="user-avatar"
            />
          </div>
          <div className="user-image-row">
            <FileUpload uploadComplete={onUploadSuccess} />
          </div>
        </div>
      </div>
    </div>
  );
};

UserProfileImage.propTypes = {
  userData: PropTypes.shape({
    avatarUrl: PropTypes.string,
  }).isRequired,
  handleUploadComplete: PropTypes.func.isRequired,
};
export default UserProfileImage;
