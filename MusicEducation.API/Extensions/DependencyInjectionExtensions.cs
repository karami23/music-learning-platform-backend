using MusicEducation.Application.Commands.Articles.Article;
using MusicEducation.Application.Commands.Articles.ArticleCategory;
using MusicEducation.Application.Commands.Authentication.ForgotPassword;
using MusicEducation.Application.Commands.Authentication.Login;
using MusicEducation.Application.Commands.Authentication.Logout;
using MusicEducation.Application.Commands.Authentication.RefreshToken;
using MusicEducation.Application.Commands.Authentication.Register;
using MusicEducation.Application.Commands.Authentication.ResetPassword;
using MusicEducation.Application.Commands.Authentication.SendVerificationCode;
using MusicEducation.Application.Commands.Authentication.VerifyPhone;
using MusicEducation.Application.Commands.Carts.Cart.AddItemToCart;
using MusicEducation.Application.Commands.Carts.Cart.ClearCart;
using MusicEducation.Application.Commands.Carts.Cart.CreateCart;
using MusicEducation.Application.Commands.Carts.Cart.RemoveItemFromCart;
using MusicEducation.Application.Commands.Commerce.DiscountCode;
using MusicEducation.Application.Commands.Commerce.Order;
using MusicEducation.Application.Commands.Commerce.Payment;
using MusicEducation.Application.Commands.Courses.Chapters.ChangeChapterOrder;
using MusicEducation.Application.Commands.Courses.Chapters.CreateChapter;
using MusicEducation.Application.Commands.Courses.Chapters.UpdateChapter;
using MusicEducation.Application.Commands.Courses.Course.ArchiveCourse;
using MusicEducation.Application.Commands.Courses.Course.ChangeCoursePrice;
using MusicEducation.Application.Commands.Courses.Course.ChangeCoursePricingType;
using MusicEducation.Application.Commands.Courses.Course.CreateCourse;
using MusicEducation.Application.Commands.Courses.Course.CreateMyCourse;
using MusicEducation.Application.Commands.Courses.Course.PublishCourse;
using MusicEducation.Application.Commands.Courses.Course.ReturnCourseToDraft;
using MusicEducation.Application.Commands.Courses.Course.UpdateCourse;
using MusicEducation.Application.Commands.Courses.Course.UpdateMyCourse;
using MusicEducation.Application.Commands.Courses.CourseCategory.ActivateCourseCategory;
using MusicEducation.Application.Commands.Courses.CourseCategory.CreateCourseCategory;
using MusicEducation.Application.Commands.Courses.CourseCategory.DeactivateCourseCategory;
using MusicEducation.Application.Commands.Courses.CourseCategory.UpdateCourseCategory;
using MusicEducation.Application.Commands.Courses.LessonMedia.ChangeLessonMediaOrder;
using MusicEducation.Application.Commands.Courses.LessonMedia.CreateLessonMedia;
using MusicEducation.Application.Commands.Courses.LessonMedia.UpdateLessonMedia;
using MusicEducation.Application.Commands.Courses.Lessons.ChangeLessonOrder;
using MusicEducation.Application.Commands.Courses.Lessons.CreateLesson;
using MusicEducation.Application.Commands.Courses.Lessons.UpdateLesson;
using MusicEducation.Application.Commands.Courses.RejectCourse;
using MusicEducation.Application.Commands.Courses.SubmitCourseForReview;
using MusicEducation.Application.Commands.Identity.Teacher.ActivateTeacher;
using MusicEducation.Application.Commands.Identity.Teacher.DeactivateTeacher;
using MusicEducation.Application.Commands.Identity.Teacher.UpdateProfile;
using MusicEducation.Application.Commands.Identity.User.ActivateUser;
using MusicEducation.Application.Commands.Identity.User.ChangePassword;
using MusicEducation.Application.Commands.Identity.User.DeactivateUser;
using MusicEducation.Application.Commands.Identity.User.PromoteUserToTeacher;
using MusicEducation.Application.Commands.Identity.User.UpdateProfile;
using MusicEducation.Application.Commands.Learning.CourseAccess.CreateCourseAccess;
using MusicEducation.Application.Commands.Learning.CourseAccess.RestoreCourseAccess;
using MusicEducation.Application.Commands.Learning.CourseAccess.RevokeCourseAccess;
using MusicEducation.Application.Commands.Learning.LessonProgress.CreateLessonProgress;
using MusicEducation.Application.Commands.Learning.LessonProgress.MarkLessonAsCompleted;
using MusicEducation.Application.Commands.Learning.LessonProgress.ResetLessonProgress;
using MusicEducation.Application.Commands.Learning.LessonProgress.UpdateLessonProgress;
using MusicEducation.Application.Commands.Notifications.CreateNotification;
using MusicEducation.Application.Commands.Notifications.MarkNotificationAsRead;
using MusicEducation.Application.Commands.Notifications.MarkNotificationAsUnread;
using MusicEducation.Application.Commands.Notifications.UpdateNotification;
using MusicEducation.Application.Commands.Reviews.ApproveReview;
using MusicEducation.Application.Commands.Reviews.CreateReview;
using MusicEducation.Application.Commands.Reviews.RejectReview;
using MusicEducation.Application.Commands.Reviews.UpdateReview;
using MusicEducation.Application.Queries.Articles.Article;
using MusicEducation.Application.Queries.Articles.ArticleCategory;
using MusicEducation.Application.Queries.Commerce.Cart;
using MusicEducation.Application.Queries.Commerce.DiscountCode;
using MusicEducation.Application.Queries.Commerce.Order;
using MusicEducation.Application.Queries.Commerce.Payment;
using MusicEducation.Application.Queries.Courses;
using MusicEducation.Application.Queries.Courses.Categories;
using MusicEducation.Application.Queries.Courses.Chapters;
using MusicEducation.Application.Queries.Courses.Lessons;
using MusicEducation.Application.Queries.Courses.Lessons.Media;
using MusicEducation.Application.Queries.Learning.CourseAccess.GetCourseAccessById;
using MusicEducation.Application.Queries.Learning.CourseAccess.GetCourseAccessByUserAndCourse;
using MusicEducation.Application.Queries.Learning.CourseAccess.GetCourseAccessesByCourseId;
using MusicEducation.Application.Queries.Learning.CourseAccess.GetCourseAccessesByUserId;
using MusicEducation.Application.Queries.Learning.LessonProgress.GetLastLessonProgress;
using MusicEducation.Application.Queries.Learning.LessonProgress.GetLessonProgressById;
using MusicEducation.Application.Queries.Learning.LessonProgress.GetLessonProgressByUserAndLesson;
using MusicEducation.Application.Queries.Learning.LessonProgress.GetLessonProgressesByLessonId;
using MusicEducation.Application.Queries.Learning.LessonProgress.GetLessonProgressesByUserId;
using MusicEducation.Application.Queries.Notifications.GetNotificationById;
using MusicEducation.Application.Queries.Notifications.GetNotificationsByUserId;
using MusicEducation.Application.Queries.Notifications.GetUnreadNotificationCount;
using MusicEducation.Application.Queries.Notifications.GetUnreadNotificationsByUserId;
using MusicEducation.Application.Queries.Reviews.GetApprovedReviewsByCourseId;
using MusicEducation.Application.Queries.Reviews.GetReviewById;
using MusicEducation.Application.Queries.Reviews.GetReviewByUserAndCourse;
using MusicEducation.Application.Queries.Reviews.GetReviewsByCourseId;
using MusicEducation.Application.Queries.Reviews.GetReviewsByUserId;

namespace MusicEducation.API.Extensions;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        // Authentication
        services.AddScoped<ForgotPasswordHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<LogoutHandler>();
        services.AddScoped<RefreshTokenHandler>();
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<ResetPasswordHandler>();
        services.AddScoped<SendVerificationCodeHandler>();
        services.AddScoped<VerifyPhoneHandler>();

        // Cart
        services.AddScoped<AddItemToCartCommandHandler>();
        services.AddScoped<ClearCartCommandHandler>();
        services.AddScoped<CreateCartCommandHandler>();
        services.AddScoped<RemoveItemFromCartCommandHandler>();

        services.AddScoped<GetCartByIdQueryHandler>();
        services.AddScoped<GetCartByUserIdQueryHandler>();

        // Discount Codes
        services.AddScoped<ActivateDiscountCodeCommandHandler>();
        services.AddScoped<CreateDiscountCodeCommandHandler>();
        services.AddScoped<DeactivateDiscountCodeCommandHandler>();
        services.AddScoped<UpdateDiscountCodeCommandHandler>();

        services.AddScoped<GetActiveDiscountCodesQueryHandler>();
        services.AddScoped<GetAllDiscountCodesQueryHandler>();
        services.AddScoped<GetDiscountCodeByCodeQueryHandler>();
        services.AddScoped<GetDiscountCodeByIdQueryHandler>();

        // Orders
        services.AddScoped<AddItemToOrderCommandHandler>();
        services.AddScoped<ApplyDiscountToOrderCommandHandler>();
        services.AddScoped<CancelOrderCommandHandler>();
        services.AddScoped<CreateOrderCommandHandler>();
        services.AddScoped<MarkOrderAsFailedCommandHandler>();
        services.AddScoped<MarkOrderAsPaidCommandHandler>();
        services.AddScoped<RemoveItemFromOrderCommandHandler>();

        services.AddScoped<GetOrderByIdQueryHandler>();
        services.AddScoped<GetOrderByOrderNumberQueryHandler>();
        services.AddScoped<GetOrdersByUserIdQueryHandler>();
        services.AddScoped<GetOrdersByUserIdAndStatusQueryHandler>();

        // Payments
        // Payments
        services.AddScoped<CancelPaymentCommandHandler>();
        services.AddScoped<CreatePaymentCommandHandler>();
        services.AddScoped<MarkPaymentAsFailedCommandHandler>();
        services.AddScoped<MarkPaymentAsPaidCommandHandler>();

        services.AddScoped<GetPaymentByIdQueryHandler>();
        services.AddScoped<GetPaymentByOrderIdQueryHandler>();
        services.AddScoped<GetPaymentByTransactionIdQueryHandler>();
        services.AddScoped<GetPaymentsByUserIdQueryHandler>();

        // Articles
        services.AddScoped<ArchiveArticleCommandHandler>();
        services.AddScoped<CreateArticleCommandHandler>();
        services.AddScoped<PublishArticleCommandHandler>();
        services.AddScoped<UnpublishArticleCommandHandler>();
        services.AddScoped<UpdateArticleCommandHandler>();

        services.AddScoped<GetAllArticlesQueryHandler>();
        services.AddScoped<GetArticleByIdQueryHandler>();
        services.AddScoped<GetArticleBySlugQueryHandler>();
        services.AddScoped<GetArticlesByCategoryIdQueryHandler>();
        services.AddScoped<GetPublishedArticlesQueryHandler>();

        // Article Categories
        services.AddScoped<ActivateArticleCategoryCommandHandler>();
        services.AddScoped<CreateArticleCategoryCommandHandler>();
        services.AddScoped<DeactivateArticleCategoryCommandHandler>();
        services.AddScoped<UpdateArticleCategoryCommandHandler>();

        services.AddScoped<GetActiveArticleCategoriesQueryHandler>();
        services.AddScoped<GetAllArticleCategoriesQueryHandler>();
        services.AddScoped<GetArticleCategoryByIdQueryHandler>();

        // Chapters
        services.AddScoped<ChangeChapterOrderHandler>();
        services.AddScoped<CreateChapterHandler>();
        services.AddScoped<UpdateChapterHandler>();

        services.AddScoped<GetAllChaptersQueryHandler>();
        services.AddScoped<GetChapterByIdQueryHandler>();
        services.AddScoped<GetChaptersByCourseIdQueryHandler>();

        // Courses
        services.AddScoped<ArchiveCourseHandler>();
        services.AddScoped<ChangeCoursePriceHandler>();
        services.AddScoped<ChangeCoursePricingTypeHandler>();
        services.AddScoped<CreateCourseHandler>();
        services.AddScoped<PublishCourseHandler>();
        services.AddScoped<ReturnCourseToDraftHandler>();
        services.AddScoped<UpdateCourseHandler>();
        services.AddScoped<RejectCourseCommandHandler>();
        services.AddScoped<SubmitCourseForReviewCommandHandler>();
        services.AddScoped<CreateMyCourseHandler>();
        services.AddScoped<UpdateMyCourseHandler>();
        services.AddScoped<GetCourseByIdHandler>();
        services.AddScoped<GetCourseBySlugHandler>();
        services.AddScoped<GetCoursesHandler>();

        // Course Categories
        services.AddScoped<ActivateCourseCategoryHandler>();
        services.AddScoped<CreateCourseCategoryHandler>();
        services.AddScoped<DeactivateCourseCategoryHandler>();
        services.AddScoped<UpdateCourseCategoryHandler>();

        services.AddScoped<GetActiveCourseCategoriesHandler>();
        services.AddScoped<GetAllCourseCategoriesHandler>();
        services.AddScoped<GetCourseCategoryByIdHandler>();

        // Lesson Media
        services.AddScoped<ChangeLessonMediaOrderCommandHandler>();
        services.AddScoped<CreateLessonMediaCommandHandler>();
        services.AddScoped<UpdateLessonMediaCommandHandler>();

        services.AddScoped<GetAllLessonMediaQueryHandler>();
        services.AddScoped<GetFirstLessonMediaQueryHandler>();
        services.AddScoped<GetLessonMediaByIdQueryHandler>();
        services.AddScoped<GetLessonMediaByLessonIdAndTypeQueryHandler>();
        services.AddScoped<GetLessonMediaByLessonIdQueryHandler>();
        services.AddScoped<GetNextLessonMediaQueryHandler>();

        // Lessons
        services.AddScoped<ChangeLessonOrderCommandHandler>();
        services.AddScoped<CreateLessonCommandHandler>();
        services.AddScoped<UpdateLessonCommandHandler>();

        services.AddScoped<GetAllLessonsQueryHandler>();
        services.AddScoped<GetLessonByIdQueryHandler>();
        services.AddScoped<GetLessonsByChapterIdQueryHandler>();



        // Teachers
        services.AddScoped<ActivateTeacherHandler>();
        services.AddScoped<DeactivateTeacherHandler>();
        services.AddScoped<UpdateTeacherProfileHandler>();

        // Users
        services.AddScoped<ActivateUserHandler>();
        services.AddScoped<ChangePasswordHandler>();
        services.AddScoped<DeactivateUserHandler>();
        services.AddScoped<PromoteUserToTeacherHandler>();
        services.AddScoped<UpdateProfileHandler>();

        // Course Access
        services.AddScoped<CreateCourseAccessCommandHandler>();
        services.AddScoped<RestoreCourseAccessCommandHandler>();
        services.AddScoped<RevokeCourseAccessCommandHandler>();

        services.AddScoped<GetCourseAccessByIdQueryHandler>();
        services.AddScoped<GetCourseAccessByUserAndCourseQueryHandler>();
        services.AddScoped<GetCourseAccessesByCourseIdQueryHandler>();
        services.AddScoped<GetCourseAccessesByUserIdQueryHandler>();

        // Lesson Progress
        services.AddScoped<CreateLessonProgressCommandHandler>();
        services.AddScoped<MarkLessonAsCompletedCommandHandler>();
        services.AddScoped<ResetLessonProgressCommandHandler>();
        services.AddScoped<UpdateLessonProgressCommandHandler>();

        services.AddScoped<GetLastLessonProgressQueryHandler>();
        services.AddScoped<GetLessonProgressByIdQueryHandler>();
        services.AddScoped<GetLessonProgressByUserAndLessonQueryHandler>();
        services.AddScoped<GetLessonProgressesByLessonIdQueryHandler>();
        services.AddScoped<GetLessonProgressesByUserIdQueryHandler>();
        // Notifications

        // Commands
        services.AddScoped<CreateNotificationCommandHandler>();
        services.AddScoped<MarkNotificationAsReadCommandHandler>();
        services.AddScoped<MarkNotificationAsUnreadCommandHandler>();
        services.AddScoped<UpdateNotificationCommandHandler>();

        // Queries
        services.AddScoped<GetNotificationByIdQueryHandler>();
        services.AddScoped<GetNotificationsByUserIdQueryHandler>();
        services.AddScoped<GetUnreadNotificationCountQueryHandler>();
        services.AddScoped<GetUnreadNotificationsByUserIdQueryHandler>();

        // Reviews

        // Commands
        services.AddScoped<ApproveReviewCommandHandler>();
        services.AddScoped<CreateReviewCommandHandler>();
        services.AddScoped<RejectReviewCommandHandler>();
        services.AddScoped<UpdateReviewCommandHandler>();

        // Queries
        services.AddScoped<GetApprovedReviewsByCourseIdQueryHandler>();
        services.AddScoped<GetReviewByIdQueryHandler>();
        services.AddScoped<GetReviewByUserAndCourseQueryHandler>();
        services.AddScoped<GetReviewsByCourseIdQueryHandler>();
        services.AddScoped<GetReviewsByUserIdQueryHandler>();

        return services;
    }
}