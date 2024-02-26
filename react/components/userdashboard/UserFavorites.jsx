import React, { useState, useEffect } from "react";
import debug from "sabio-debug";
import UserFavoriteCard from "./UserFavoriteCard";
import { GetByUserId } from "services/venueService";
import PropTypes from "prop-types";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import "../userdashboard/userDashboardStyling.css";
const _logger = debug.extend("UserFavorites");

function UserFavorites({ currentUser }) {
  _logger(currentUser);

  const [pageData, setPageData] = useState({
    arrayOfVenues: [],
    favoritedVenues: [],
  });

  useEffect(() => {
    fetchUserVenues();
  }, []);

  const fetchUserVenues = () => {
    GetByUserId(currentUser.id)
      .then(onUserVenuesSuccess)
      .catch(onUserVenuesError);
  };

  const onFavoriteDeleteHandler = (venueId) => {
    setPageData((prevState) => {
      const updatedArray = prevState.arrayOfVenues.filter(
        (venue) => venue.id !== venueId
      );

      const newState = {
        ...prevState,
        arrayOfVenues: updatedArray,
        favoritedVenues: updatedArray.map(mapArray),
      };
      return newState;
    });
  };

  _logger(pageData);

  const onUserVenuesSuccess = (response) => {
    let venuesArray = response.items;

    setPageData((prevState) => ({
      ...prevState,
      arrayOfVenues: venuesArray,
      favoritedVenues: venuesArray.map(mapArray),
    }));
  };

  const onUserVenuesError = (error) => {
    _logger("error", error);
  };

  const mapArray = (aVenue) => {
    return (
      <div className="col-lg-2 user-favorite-card-map">
        <UserFavoriteCard
          venue={aVenue}
          key={aVenue.id}
          deleteFavorite={onFavoriteDeleteHandler}
        />
      </div>
    );
  };

  return (
    <React.Fragment>
      <div className="user-favorite-venues container-fluid p-4">
        <div className="row">
          <div className="col-12">
            <div className="card-header d-flex justify-content-between align-items-center"></div>
            <div>
              <h1 className="mb-0 border-bottom">Your Favorite Venues:</h1>
            </div>
            <div className="row user-favorite-slider">
              <Slider
                slidesToShow={4}
                infinite={false}
                autoplay={true}
                autoplaySpeed={3000}
              >
                {pageData.favoritedVenues}
              </Slider>
            </div>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
}
UserFavorites.propTypes = {
  currentUser: PropTypes.shape({
    id: PropTypes.number.isRequired,
  }),
  onFavoriteDeleteHandler: PropTypes.func,
  venue: PropTypes.shape({
    id: PropTypes.number.isRequired,
    organizationId: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    url: PropTypes.string.isRequired,
    venueType: PropTypes.shape({
      name: PropTypes.string.isRequired,
    }),
    location: PropTypes.shape({
      lineOne: PropTypes.string.isRequired,
      lineTwo: PropTypes.string.isRequired,
      city: PropTypes.string.isRequired,
      zip: PropTypes.string.isRequired,
      state: PropTypes.shape({
        name: PropTypes.string.isRequired,
      }),
    }),
  }),
};

export default UserFavorites;
