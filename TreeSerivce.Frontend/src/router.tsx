import { Button } from "@mui/material";
import { createBrowserRouter } from "react-router-dom";
import { RootLayout } from "./components/RootLayout";

export const router = createBrowserRouter([
	{
		path: "/",
		element: <RootLayout />,
	},
	{
		path: "*",
		element: (
			<div className="flex flex-col items-center justify-center min-h-screen bg-gray-100">
				<h1 className="text-3xl font-bold mb-4">Page Not Found (</h1>
				<Button
					variant="outlined"
					color="primary"
					className="bg-blue-500"
					onClick={() => window.history.back()}
				>
					Go Back
				</Button>
			</div>
		),
	},
]);
