import dotenv from 'dotenv';

// Set the NODE_ENV to 'development' by default
process.env.NODE_ENV = process.env.NODE_ENV || 'development';

const envFound = dotenv.config();
if (!envFound) {
  // This error should crash whole process

  throw new Error("⚠️  Couldn't find .env file  ⚠️");
}

export default {
  /**
   * Your favorite port
   */
  port: parseInt(process.env.PORT, 10) || 2000,

  /**
   * That long string from mlab
   */
  //databaseURL: process.env.MONGODB_URI || "mongodb://lapr5g50mdb:zwQzAFGDOr6rtkbJLhqVRRxB5WKdX5nkg9F071nzCjtQ3yPivRk07kbgzrit9xbvrXeo41kZZnTJSB6HyuGbEg==@lapr5g50mdb.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&maxIdleTimeMS=120000&appName=@lapr5g50mdb@",
  databaseURL: process.env.MONGODB_URI || "mongodb://lapr5g50mdb:zwQzAFGDOr6rtkbJLhqVRRxB5WKdX5nkg9F071nzCjtQ3yPivRk07kbgzrit9xbvrXeo41kZZnTJSB6HyuGbEg==@lapr5g50mdb.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&maxIdleTimeMS=120000&appName=@lapr5g50mdb@",

  /**
   * Your secret sauce
   */
  jwtSecret: process.env.JWT_SECRET || "my sakdfho2390asjod$%jl)!sdjas0i secret",

  /**
   * Used by winston logger
   */
  logs: {
    level: process.env.LOG_LEVEL || 'info',
  },

  /**
   * API configs
   */
  api: {
    prefix: '/api',
  },

  controllers: {
    role: {
      name: "RoleController",
      path: "../controllers/roleController"
    },
    post: {
      name: "PostController",
      path: "../controllers/postController"
    },
    reaction: {
      name: "ReactionController",
      path: "../controllers/reactionController"
    }
  },

  repos: {
    post: {
      name: "PostRepo",
      path: "../repos/postRepo"
    },
    role: {
      name: "RoleRepo",
      path: "../repos/roleRepo"
    },
    user: {
      name: "UserRepo",
      path: "../repos/userRepo"
    }
  },

  services: {
    post: {
      name: "PostService",
      path: "../services/postService"
    },
    role: {
      name: "RoleService",
      path: "../services/roleService"
    },
    reaction: {
      name: "ReactionService",
      path: "../services/reactionService"
    }
  },
};
