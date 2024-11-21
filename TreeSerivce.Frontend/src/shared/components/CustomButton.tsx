import Button from "@mui/material/Button";
import React from "react";

interface ButtonProps {
	children: React.ReactNode;
	onClick: () => void;
	className?: string;
}

const CustomButton: React.FC<ButtonProps> = ({
	children,
	onClick,
	className,
}) => (
	<Button
		onClick={onClick}
		variant="contained"
		color="primary"
		className={`rounded-md shadow-md ${className}`}
		sx={{
			backgroundColor: "primary.main",
			"&:hover": { backgroundColor: "secondary.main" },
		}}
	>
		{children}
	</Button>
);

export default CustomButton;
