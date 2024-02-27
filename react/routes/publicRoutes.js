import { lazy } from "react";
const Landing = lazy(() => import("../components/landing/Landing"));
const ShareStories = lazy(() =>
  import("../components/sharestories/ShareStories")
);
const PageNotFound = lazy(() => import("../components/error/Error404"));
const Privacy = lazy(() => import("../components/privacy/Privacy"));
const About = lazy(() => import("../components/pages/About"));
const FileTest = lazy(() => import("../components/files/FileTest"));
const FileUpload = lazy(() => import("../components/files/FileUpload"));
const Faqs = lazy(() => import("../components/faqspage/Faqs"));
const Register = lazy(() => import("../components/user/Register"));
const RegisterEmployee = lazy(() =>
  import("../components/user/RegisterEmployee")
);
const Login = lazy(() => import("../components/user/Login"));
const ConfirmUser = lazy(() => import("../components/user/ConfirmUser"));
const Ratings = lazy(() => import("../components/ratings/RatingForm"));
const UnsubscribeNewsletter = lazy(() =>
  import("../components/newsletter/UnsubscribeNewsletter")
);
const NewsLetter = lazy(() => import("../components/newsletter/NewsLetter"));
const Support = lazy(() => import("../components/support/Support"));
const Blogs = lazy(() => import("../components/blogspublic/blogs"));
const BlogDetails = lazy(() => import("../components/blogspublic/blogDetails"));

const RenderVenues = lazy(() => import("../components/venues/RenderVenues"));
const VenueDetails = lazy(() => import("../components/venues/VenueDetails"));
const ResetPassword = lazy(() => import("../components/user/ResetPassword"));
const ChangePassword = lazy(() => import("../components/user/ChangePassword"));
const Podcasts = lazy(() => import("../components/pages/Podcasts"));
const CookiePolicy = lazy(() => import("../components/cookies/CookiePolicy"));


const routes = [
  {
    path: "/unsubscribenewsletter",
    name: "Unsubscribe Newsletter",
    exact: true,
    element: UnsubscribeNewsletter,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/",
    name: "Landing",
    exact: true,
    element: Landing,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/sharestory",
    name: "Share Story",
    exact: true,
    element: ShareStories,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/privacy",
    name: "Privacy",
    exact: true,
    element: Privacy,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/blogs",
    name: "blogs",
    exact: true,
    element: Blogs,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/blogs/:id",
    name: "BlogDetails",
    exact: true,
    element: BlogDetails,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/ratings",
    name: "Ratings",
    exact: true,
    element: Ratings,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/file",
    name: "FileTest",
    exact: true,
    element: FileTest,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/fileupload",
    name: "fileupload",
    exact: true,
    element: FileUpload,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/faqs",
    name: "Faqs",
    exact: true,
    element: Faqs,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/about",
    name: "About",
    exact: true,
    element: About,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/confirmuser",
    name: "ConfirmUser",
    exact: true,
    element: ConfirmUser,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/register",
    name: "Register",
    exact: true,
    element: Register,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/registeremployee",
    name: "RegisterEmployee",
    exact: true,
    element: RegisterEmployee,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/login",
    name: "Login",
    exact: true,
    element: Login,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/support",
    name: "Contact Us",
    exact: true,
    element: Support,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/venues",
    name: "RenderVenues",
    exact: true,
    element: RenderVenues,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/venue/:id",
    name: "VenueDetails",
    exact: true,
    element: VenueDetails,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/newsletter",
    name: "NewsLetter",
    exact: true,
    element: NewsLetter,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/resetpassword",
    name: "ResetPassword",
    exact: true,
    element: ResetPassword,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/changepassword",
    name: "ChangePassword",
    exact: true,
    element: ChangePassword,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/podcasts",
    name: "Podcasts",
    exact: true,
    element: Podcasts,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/cookiepolicy",
    name: "Cookie Policy",
    exact: true,
    element: CookiePolicy,
    roles: [],
    isAnonymous: true,
  },
];

const errorRoutes = [
  {
    path: "*",
    name: "Error - 404",
    element: PageNotFound,
    roles: [],
    exact: true,
    isAnonymous: true,
  },
];

var allRoutes = [...routes, ...errorRoutes];

export default allRoutes;
