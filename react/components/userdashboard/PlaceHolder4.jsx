import React from "react";

const PlaceHolder4 = () => {
  return (
    <div className={`col-12`}>
      <div className="card mb-4">
        <div className="card-body">
          <div className="d-flex align-items-center justify-content-between mb-3 lh-1">
            <div>
              <span className="fs-6 text-uppercase fw-semibold ls-md">
                Place Holder for Later 4
              </span>
            </div>
            <div>
              <span className="fe fe-user-check fs-3 text-primary"></span>
            </div>
          </div>
          <h2 className="fw-bold mb-1">Placeholder</h2>
          <span className="text-success fw-semibold">
            <i className="fe fe-trending-up me-1"></i>
            Placeholder
          </span>
          <span className="ms-1 fw-medium">Placeholder</span>
        </div>
      </div>
    </div>
  );
};

export default PlaceHolder4;
