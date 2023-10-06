// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.


export const environment = {
  production: false,
  endpoints: {
    apiBaseURL: 'https://localhost:7269/',
    login: 'api/v1.0/lms/user/login',
    addUser: 'api/v1.0/lms/user/register',
    getCourse: 'api/v1.0/lms/courses/getall',
    addCourse: 'api/v1.0/lms/courses/add',
    deleteCourse: 'api/v1.0/lms/courses/delete/{coursename}',
    getCourseByTechnology:'api/v1.0/lms/courses/info/{technology}',
    getCourseByDuration:'api/v1.0/lms/courses/get/{technology}/{durationFromRange}/{durationToRange}'
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
