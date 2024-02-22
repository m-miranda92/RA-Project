import React, { useState } from "react";
import PropTypes from "prop-types";
import debug from "sabio-debug";
import { updateDemographics } from "services/userService";
import toastr from "toastr";
import "../userdashboard/userDashboardStyling.css";
import { useNavigate } from "react-router-dom";

const _logger = debug.extend("UserDemographicsForm");

const UserDemographicsForm = (props) => {
  const navigate = useNavigate();
  _logger(props);
  const [isFormChanged, setIsFormChanged] = useState(false);
  const handleInputChange = (e) => {
    const { name, value } = e.target;
    props.setUserData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
    setIsFormChanged(true);
  };
  const handleResetPasswordClick = () => {
    navigate("/resetpassword");
  };
  const handleUpdateUserInfo = (e) => {
    e.preventDefault();
    const payload = {
      id: props.userData.id,
      firstName: props.userData.firstName,
      mi: props.userData.mi,
      lastName: props.userData.lastName,
      email: props.userData.email,
      avatarUrl: props.userData.avatarUrl,
    };
    updateDemographics(payload).then(onUpdateSuccess).catch(onUpdateError);
  };
  const onUpdateSuccess = (response) => {
    toastr.success("Update Successful!");
    setIsFormChanged(false);
    _logger(
      "Success from UserDemographicsForm upon calling onUpdateSuccess: ",
      response
    );
  };
  const onUpdateError = (error) => {
    toastr.error("Update unsuccessful, check information and try again.");
    _logger(
      "Error from UserDemographicsForm upon calling onUpdateSuccess: ",
      error
    );
  };
  return (
    <div className="col-12">
      <div className="card mb-4">
        <div className="card-header align-items-center card-header-height d-flex justify-content-between align-items-center">
          <div className="d-flex justify-content-between">
            <h2 className="mb-0 form-title-userdashboard">User Demographics</h2>
            <button
              className="btn btn-danger"
              onClick={handleResetPasswordClick}
            >
              Reset Password
            </button>
          </div>
        </div>
        <div className="card-body">
          <form>
            <div className="row">
              <div className="col-md-5 mb-3">
                <label
                  htmlFor="firstname"
                  className="form-label"
                >
                  First Name:
                </label>
                <input
                  type="text"
                  className="form-control"
                  name="firstName"
                  id="firstname"
                  aria-describedby="firstname"
                  value={props.userData.firstName}
                  onChange={handleInputChange}
                />
              </div>
              <div className="col-md-2 mb-3">
                <label
                  htmlFor="middleinitial"
                  className="form-label"
                >
                  Middle Initial:
                </label>
                <input
                  type="text"
                  className="form-control"
                  name="mi"
                  id="middleinitial"
                  aria-describedby="middleinitial"
                  value={props.userData.mi}
                  onChange={handleInputChange}
                />
              </div>
              <div className="col-md-5 mb-3">
                <label
                  htmlFor="lastname"
                  className="form-label"
                >
                  Last Name:
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="lastName"
                  name="lastName"
                  aria-describedby="lastname"
                  value={props.userData.lastName}
                  onChange={handleInputChange}
                />
              </div>
              <div className="userdashboard-email-row">
                <div className="user-container row">
                  <div className="col-md-9 mb-3">
                    <label
                      htmlFor="email"
                      className="form-label"
                    >
                      Email:
                    </label>
                    <input
                      type="email"
                      className="form-control"
                      name="email"
                      id="email"
                      aria-describedby="email"
                      value={props.userData.email}
                      onChange={handleInputChange}
                    />
                  </div>
                  <div className="col-md-3 mt-5 mb-4 d-flex align-items-center justify-content-end">
                    {isFormChanged && (
                      <button
                        className="btn btn-success"
                        onClick={handleUpdateUserInfo}
                        disabled={!isFormChanged}
                      >
                        Update
                      </button>
                    )}
                  </div>
                </div>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

UserDemographicsForm.propTypes = {
  userData: PropTypes.shape({
    id: PropTypes.number.isRequired,
    firstName: PropTypes.string.isRequired,
    mi: PropTypes.string.isRequired,
    lastName: PropTypes.string.isRequired,
    email: PropTypes.string.isRequired,
    avatarUrl: PropTypes.string.isRequired,
  }).isRequired,
  handleInputChange: PropTypes.func,
  handleUpdateUserInfo: PropTypes.func,
  isFormChanged: PropTypes.bool,
  setUserData: PropTypes.func.isRequired,
};
export default UserDemographicsForm;
