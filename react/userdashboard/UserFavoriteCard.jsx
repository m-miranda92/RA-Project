import React from "react";
import debug from "sabio-debug";
import PropTypes from "prop-types";
import { FaHeart } from "react-icons/fa";
import "../userdashboard/userDashboardStyling.css";
import { DeleteFavorite } from "services/venueService";

const _logger = debug.extend("UserFavoriteCard");

function UserFavoriteCard(props) {
  _logger("This is incoming props: ", props);
  const aVenue = props.venue;
  const maxDescriptionCharacters = 100;

  const truncatedDescription =
    aVenue.description.length > maxDescriptionCharacters
      ? aVenue.description.substring(0, maxDescriptionCharacters) + "..."
      : aVenue.description;

  _logger(aVenue);

  const handleDeleteFavorite = () => {
    DeleteFavorite(props.venue.id).then(onDeleteSuccess).catch(onDeleteError);
  };
  const onDeleteSuccess = (response) => {
    props.deleteFavorite(props.venue.id);
    _logger("Delete Favorite was successful: ", response);
  };
  const onDeleteError = (error) => {
    _logger("Delete Favorite was unsuccessful: ", error);
  };
  return (
    <div className="card-container">
      <div className="col-12">
        <div className="card-header align-items-center card-header-height d-flex justify-content-between align-items-center"></div>
        <div className="user-favorited card mr-3">
          <img
            src={aVenue.url}
            className="card-img-top max-size-image"
            alt={aVenue.organizationId}
          />
          <div className="card-body">
            <h2 className="card-title">{aVenue.name}</h2>
            <h5 className="card-text">{truncatedDescription}</h5>
            <p className="card-text">
              {" "}
              {aVenue.location.lineOne} {aVenue.location.lineTwo}{" "}
            </p>
            <p className="card-text">
              {aVenue.location.city} {aVenue.location.zip}
            </p>
            <p className="card-text">{aVenue.location.state.name}</p>
            <p className="card-text">
              {" "}
              <strong>{aVenue.venueType.name}</strong>
            </p>
          </div>
          <div className="card-footer user-favorite-venue-footer">
            <button>
              <FaHeart
                className="heart-icon-favorites-card"
                onClick={handleDeleteFavorite}
              />
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}

UserFavoriteCard.propTypes = {
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
  deleteFavorite: PropTypes.func,
};

export default UserFavoriteCard;
