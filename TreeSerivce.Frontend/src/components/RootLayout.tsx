import { Outlet } from "react-router-dom";

import { createTheme, ThemeProvider } from "@mui/material/styles";
import { TreePage } from "../pages/TreePage";
const theme = createTheme({
	palette: {
		primary: { main: "#fb923c" },
		secondary: { main: "#f97316" },
		background: { default: "#fef3c7" },
		text: { primary: "#000000", secondary: "#4b5563" },
	},
});

export function RootLayout() {
	return (
		<ThemeProvider theme={theme}>
			<Outlet />
			<TreePage />
		</ThemeProvider>
	);
}
