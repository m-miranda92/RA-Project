import React, { useEffect, useState } from "react";
import UserHeader from "./UserHeader";
import PlaceHolder1 from "./PlaceHolder1";
import PlaceHolder2 from "./PlaceHolder2";
import PlaceHolder3 from "./PlaceHolder3";
import PlaceHolder4 from "./PlaceHolder4";
import UserDemographicsForm from "./UserDemographicsForm";
import UserProfileImage from "./UserProfileImage";
import UserFavorites from "./UserFavorites";
import PropTypes from "prop-types";
import debug from "sabio-debug";
import { getByEmail } from "../../services/userService";

const _logger = debug.extend("UserDashboard");

function UserDashboard({ currentUser }) {
  _logger(currentUser);
  const [userData, setUserData] = useState({
    firstName: "",
    mi: "",
    lastName: "",
    email: "",
    id: "",
    avatarUrl: "",
  });

  useEffect(() => {
    if (currentUser) {
      getByEmail(currentUser.email)
        .then(onGetCurrentSuccess)
        .catch(onGetCurrentError);
    }
  }, [currentUser]);

  const onGetCurrentSuccess = (response) => {
    setUserData({
      firstName: response.item.firstName,
      mi: response.item.mi || "",
      lastName: response.item.lastName,
      email: response.item.email,
      avatarUrl: response.item.avatarUrl || "",
      id: response.item.id,
    });
    _logger(response);
  };

  const onGetCurrentError = (error) => {
    _logger("Error from userDashboard upon calling onGetCurrentError: ", error);
  };
  const handleUploadComplete = (response) => {
    _logger(
      "Response from userDashboard upon calling handleUploadComplete: ",
      response.items[0].url
    );
    setUserData((prevState) => {
      const pD = { ...prevState };
      pD.avatarUrl = response.items[0].url;
      return pD;
    });
  };

  return (
    <div className="container-fluid p-4">
      <UserHeader />
      <div className="row">
        <div className="col-3">
          <PlaceHolder1 currentUser={currentUser} />
        </div>
        <div className="col-3">
          <PlaceHolder2 />
        </div>
        <div className="col-3">
          <PlaceHolder3 />
        </div>
        <div className="col-3">
          <PlaceHolder4 />
        </div>
      </div>
      <div className="row">
        <div className="col-9">
          <UserDemographicsForm
            userData={userData}
            setUserData={setUserData}
          />
        </div>
        <div className="col-3">
          <UserProfileImage
            userData={userData}
            handleUploadComplete={handleUploadComplete}
          />
        </div>
      </div>
      <div className="row">
        <div className="col-12">
          <UserFavorites currentUser={currentUser} />
        </div>
      </div>
    </div>
  );
}

UserDashboard.propTypes = {
  currentUser: PropTypes.shape({
    email: PropTypes.string,
    role: PropTypes.string,
    id: PropTypes.number,
    firstName: PropTypes.string,
    lastName: PropTypes.string,
    mi: PropTypes.string,
    avatarUrl: PropTypes.string,
    isLoggedIn: PropTypes.bool,
  }),
};

export default UserDashboard;
