import React, { useEffect, useState } from "react";
import { FaRegCalendar } from "react-icons/fa";
import { getByCreatedBy } from "services/reservationService";
import debug from "sabio-debug";
import PropTypes from "prop-types";

const _logger = debug.extend("User Dashboard Reservations: ");

function UserPlaceHolder({ currentUser }) {
  _logger("This is the currentUser: ", currentUser);
  const [userResData, setUserResData] = useState({
    id: "",
    couponCode: "",
    discountValue: "",
    total: "",
    chargeId: "",
    reservationDate: "",
    startTime: "",
    tableId: "",
    bookingStatusName: "",
    paymentAccountId: "",
    billingAddress: "",
    venueName: "",
  });

  const onGetReservationSuccess = (response) => {
    _logger(response);

    setUserResData((prevState) => {
      const res = response.items[0];

      const pD = { ...prevState };
      pD.id = res.id;
      pD.couponCode = res.couponCode;
      pD.discountValue = res.discountValue;
      pD.total = res.total;
      pD.chargeId = res.chargeId;
      pD.reservationDate = res.reservationDate;
      pD.tableId = res.tableId;
      pD.bookingStatusName = res.bookingStatus.id;
      pD.paymentAccountId = res.paymentAccountId;
      pD.billingAddress = res.billingAddress;
      pD.venueName = res.venueName;
      return pD;
    });

    _logger("Updated state: ", userResData);
  };
  useEffect(() => {
    getByCreatedBy().then(onGetReservationSuccess).catch(onGetReservationError);
  }, []);

  const onGetReservationError = (error) => {
    _logger(error);
  };
  const reservationDate = new Date(userResData.reservationDate);
  const formattedDate = reservationDate.toLocaleDateString("en-US", {
    month: "2-digit",
    day: "2-digit",
    year: "numeric",
  });
  return (
    <div className={`col-12`}>
      <div className="card mb-4">
        <div className="card-body">
          <div className="d-flex align-items-center justify-content-between mb-3 lh-1">
            <div>
              <span className="fs-6 text-uppercase fw-semibold ls-md">
                Your Upcoming Reservations:
              </span>
            </div>
            <div>
              <button className="reservation-icon-styling">
                <FaRegCalendar className="reservation-user-dashboard-icon" />
              </button>
            </div>
          </div>
          <h3 className="fw-bold mb-1">Date: {formattedDate}</h3>
          <button className="text-success fw-semibold user-reservation-venue-name">
            {userResData.venueName}
          </button>
        </div>
      </div>
    </div>
  );
}

export default UserPlaceHolder;

UserPlaceHolder.propTypes = {
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
