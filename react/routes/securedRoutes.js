import { lazy } from "react";

const AnalyticsDashboards = lazy(() =>
  import("../components/dashboard/analytics/Analytics")
);
const FileManager2 = lazy(() =>
  import("components/dashboard/analytics/filemanager/FileManager")
);
const PageNotFound = lazy(() => import("../components/error/Error404"));
const Comments = lazy(() => import("../components/comments/Comments"));
const PaymentAccountDashboard = lazy(() =>
  import(
    "../components/dashboard/analytics/paymentaccounts/PaymentAccountDashboard"
  )
);
const StripeCreateAccountSuccess = lazy(() =>
  import(
    "../components/dashboard/analytics/paymentaccounts/StripeCreateAccountSuccess"
  )
);
const PaymentDetails = lazy(() =>
  import(
    "../components/dashboard/analytics/paymentaccounts/PaymentAccountDetails"
  )
);
const CheckoutDetails = lazy(() =>
  import("../components/checkout/CheckoutDetails")
);
const BlogsFormId = lazy(() => import("../components/blogs/BlogForm"));
const BlogsForm = lazy(() => import("../components/blogs/BlogForm"));

const DashboardUser = lazy(() =>
  import("../components/userdashboard/DashboardUser")
);
const CheckoutSuccess = lazy(() =>
  import("../components/checkout/CheckoutSuccess")
);
const Product = lazy(() => import("../components/product/Product"));
const OrganizationManager = lazy(() =>
  import("../components/organizations/OrganizationManager")
);
const CreateNewLink = lazy(() =>
  import("../components/linksexternal/CreateNewLinkForm")
);
const ExternalLinks = lazy(() =>
  import("../components/linksexternal/LinkListTable")
);
const FaqsList = lazy(() => import("../components/faqs/FaqsList"));
const FaqAddEdit = lazy(() => import("../components/faqs/FaqAddEdit"));
const ShareStoryForm = lazy(() =>
  import("../components/sharestories/ShareStoryForm")
);

const BusinessConfirmation = lazy(() =>
  import("../components/user/BusinessConfirmation")
);

const DiscountTable = lazy(() =>
  import("../components/discount/DiscountTable")
);
const NewsPaperSubscriptionsAdmin = lazy(() =>
  import("../components/newsletter/NewsletterSubscriptionsAdmin")
);
const Reservation = lazy(() =>
  import("../components/reservation/ReservationPage")
);

const DiscountsForm = lazy(() =>
  import("../components/discounts/DiscountsForm")
);
const businessDash = lazy(() =>
  import("../components/businessdash/BusinessDash")
);
const AddVenue = lazy(() => import("../components/venues/AddVenue"));
const TableListAdmin = lazy(() =>
  import("../components/tables/TableListAdmin")
);

const AdminDashboard = lazy(() =>
  import("../components/dashboard/analytics/admindashboard/AdminDashboard")
);

const TableFormAdmin = lazy(() => import("../components/tables/TableFormAdmin"));

const TableFormBusiness = lazy (() => import ("../components/tables/TableFormBusiness"));

const OrganizationDetailView = lazy(() =>
  import("../components/organizations/OrganizationDetailView")
);

const ShareStoriesTable = lazy(() =>
  import("../components/sharestories/ShareStoriesTable")
);

const EventsList = lazy(() => import("../components/eventslist/EventsList"));
const EventsDetails = lazy(() => import("../components/events/EventsDetails"));
const Eventsform = lazy(() => import("../components/events/EventsForm"));

const PodcastsForm = lazy(()=> import("../components/pages/PodcastsForm"));

const dashboardRoutes = [
  {
    path: "/dashboard",
    name: "Dashboards",
    icon: "uil-home-alt",
    header: "Navigation",
    children: [
      {
        path: "/dashboard/admin",
        name: "AdminDashboard",
        element: AdminDashboard,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/dashboard/analytics",
        name: "Analytics",
        element: AnalyticsDashboards,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/tableslist",
        name: "TableList",
        exact: true,
        element: TableListAdmin,
        roles: ["Admin"],
        isAnonymous: false,
      },
      {
        path: "/storiestable",
        name: "Share Stories Table",
        exact: true,
        element: ShareStoriesTable,
        roles: ["Admin"],
        isAnonymous: false,
      },
      {
        path: "/dashboard/paymentaccounts",
        name: "PaymentAccountDashboard",
        element: PaymentAccountDashboard,
        roles: ["Admin", "Business"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/dashboard/user",
        name: "DashboardUser",
        element: DashboardUser,
        roles: ["Admin", "User"],
        exact: true,
        isAnonymous: false,
      },

      {
        path: "/dashboard/files",
        name: "FileManager2",
        element: FileManager2,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/faqslist",
        name: "FaqsList",
        exact: true,
        element: FaqsList,
        roles: ["Admin"],
        isAnonymous: false,
      },
      {
        path: "/faqaddedit",
        name: "FaqAdd",
        exact: true,
        element: FaqAddEdit,
        roles: ["Admin"],
        isAnonymous: false,
      },
      {
        path: "/faqaddedit/:id",
        name: "FaqEdit",
        exact: true,
        element: FaqAddEdit,
        roles: ["Admin"],
        isAnonymous: false,
      },
      {
        path: "/dashboard/business",
        name: "BusinessDash",
        element: businessDash,
        roles: ["Business"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/addvenue",
        name: "AddVenue",
        element: AddVenue,
        roles: ["Business"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/organizationmanager",
        name: "OrganizationManager",
        exact: true,
        element: OrganizationManager,
        roles: ["Admin"],
        isAnonymous: false,
      },
      {
        path: "/checkoutdetails",
        name: "CheckoutDetails",
        element: CheckoutDetails,
        roles: ["Admin", "User"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/checkoutsuccess",
        name: "CheckoutSuccess",
        element: CheckoutSuccess,
        roles: ["Admin", "User"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/reservation",
        name: "Reservation",
        element: Reservation,
        roles: ["Admin", "User"],
        exact: true,
        isAnonymouse: false,
      },
      {
        path: "/product",
        name: "Product",
        element: Product,
        roles: ["Admin", "User"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/comments",
        name: "Comments",
        exact: true,
        element: Comments,
        roles: ["Admin", "User"],
        isAnonymous: false,
      },
      {
        path: "/blogsform/:id",
        name: "BlogsForm",
        exact: true,
        element: BlogsFormId,
        roles: ["Admin"],
        isAnonymous: false,
      },
      {
        path: "/blogsform",
        name: "BlogsForm",
        exact: true,
        element: BlogsForm,
        roles: ["Admin"],
        isAnonymous: false,
      },
      {
        path: "/discount",
        name: "DiscountTable",
        exact: true,
        element: DiscountTable,
        roles: ["Business"],
        isAnonymous: false,
      },
      {
        path: "/paymentaccounts/create/success",
        name: "StripeCreateAccountSuccess",
        exact: true,
        element: StripeCreateAccountSuccess,
        roles: ["Business"],
        isAnonymous: false,
      },
      {
        path: "/paymentdetails",
        name: "PaymentDetails",
        exact: true,
        element: PaymentDetails,
        roles: ["Admin", "User"],
        isAnonymous: false,
      },
      {
        path: "/businessconfirmation",
        name: "BusinessConfirmation",
        exact: true,
        element: BusinessConfirmation,
        roles: ["Admin"],
        isAnonymous: true,
      },
      {
        path: "/organizations/:id",
        name: "OrganizationDetailView",
        exact: true,
        element: OrganizationDetailView,
        roles: ["Admin"],
        isAnonymous: true,
      },
      {
        path: "/links",
        name: "ExternalLinks",
        exact: true,
        element: ExternalLinks,
        roles: ["Admin", "Business"],
        isAnonymous: false,
      },
      {
        path: "/links/createnewlink",
        name: "CreateNewLink",
        exact: true,
        element: CreateNewLink,
        roles: ["Admin", "Business"],
        isAnonymous: false,
      },
      {
        path: "/storyform",
        name: "StoryForm",
        exact: true,
        element: ShareStoryForm,
        roles: ["Admin"],
        isAnonymous: false,
      },
      {
        path: "/newslettersubscriptionsadmin",
        name: "NewsPaperSubscriptionsAdmin",
        exact: true,
        element: NewsPaperSubscriptionsAdmin,
        roles: ["Admin"],
        isAnonymous: false,
      },
      {
        path: "/discountsform",
        name: "Discount Form",
        exact: true,
        element: DiscountsForm,
        roles: ["Business"],
        isAnonymous: false,
      },
      {
        path: "/tableform/add",
        name: "TableFormAdmin",
        exact: true,
        element: TableFormAdmin,
        roles: ["Admin"],
        isAnonymous: false,
      },
      {
        path: "/tablesform",
        name: "TableFormBusiness",
        exact: true,
        element: TableFormBusiness,
        roles: ["Business", "Employee"],
        isAnonymous: false,
      },
      {
        path: "/events/details/:Id",
        name: "Events Details",
        exact: true,
        element: EventsDetails,
        roles: ["Admin", "Business", "Employee"],
        isAnonymous: true,
      },
      {
        path: "/eventslists",
        name: "Events List",
        exact: true,
        element: EventsList,
        roles: ["Admin", "Business", "Employee"],
        isAnonymous: true,
      },
      {
        path: "/eventsform",
        name: "Eventsform",
        exact: true,
        element: Eventsform,
        roles: ["Admin", "Business", "Employee"],
        isAnonymous: true,
      },
      {
        path: "/podcasts/add",
        name: "PodcastsForm",
        exact: true,
        element: PodcastsForm,
        roles: ["Admin"],
        isAnonymous: false,
      },
    ],
    roles: ["Admin", "User", "Business"],
  },
];
const test = [
  {
    path: "/test",
    name: "Test",
    exact: true,
    element: AnalyticsDashboards,
    roles: ["Fail"],
    isAnonymous: false,
  },
  {
    path: "/secured",
    name: "A Secured Route",
    exact: true,
    element: AnalyticsDashboards,
    roles: ["Fail"],
    isAnonymous: false,
  },
  {
    path: "/secured2",
    name: "A Secured Route",
    exact: true,
    element: AnalyticsDashboards,
    roles: ["Admin"],
    isAnonymous: false,
  },
];
const errorRoutes = [
  {
    path: "*",
    name: "Error - 404",
    element: PageNotFound,
    roles: [],
    exact: true,
    isAnonymous: false,
  },
];

const allRoutes = [...dashboardRoutes, ...test, ...errorRoutes];
export default allRoutes;
